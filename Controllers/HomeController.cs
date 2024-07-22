using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using logregDemo1.Models;
using Microsoft.AspNetCore.Identity;
//this is for using session
using Microsoft.AspNetCore.Http;

namespace logregDemo1.Controllers;

public class HomeController : Controller
{
    private MyContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        //Clear the session whenever we go to the index page
        HttpContext.Session.Clear();
        return View();
    }

    [HttpPost("user/register")]
    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid)
        {
            //we passed validations
            //we need to check if the email is unique
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                //We have a problem. This email is already in the database
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Index");
            }
            else
            {
                // Hash the password
                PasswordHasher<User> Hasher = new();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

                // Save the user to the database
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Save the user id to session
                HttpContext.Session.SetInt32("user", newUser.UserId);

                return RedirectToAction("Success");
            }
        }
        else
        {
            return View("Index");
        }
    }

    [HttpPost("user/login")]
    public IActionResult Login(LogUser loginUser)
    {
        if (ModelState.IsValid)
        {
            //Step 1: Check if the email exists in the database
            User userInDb = _context.Users.FirstOrDefault(u => u.Email == loginUser.LogEmail);
            //If it does not exist, we have a problem
            if (userInDb == null)
            {
                //We have a problem. This email is not in the database
                ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                return View("Index");
            }

            var hasher = new PasswordHasher<LogUser>();

            var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LogPassword);

            if (result == 0)
            {
                //We have a problem. The password is incorrect
                ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                return View("Index");
            }
            else
            {
                // Save the user id to session
                HttpContext.Session.SetInt32("user", userInDb.UserId);
                //We have a successful login
                return RedirectToAction("Success");
            }
        }
        else
        {
            return View("Index");
        }
    }

    [HttpGet("Success")]
    public IActionResult Success()
    {
        if (HttpContext.Session.GetInt32("user") == null)
        {
            return RedirectToAction("Index");
        }
        // Check if the user is logged in
        User loggedInUser = _context.Users.FirstOrDefault(u => u.UserId == (int) HttpContext.Session.GetInt32("user"));       
        // if user is null, then the user is not logged in
        if (loggedInUser == null)
        {
            return RedirectToAction("Index");
        }
        return View(loggedInUser);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
