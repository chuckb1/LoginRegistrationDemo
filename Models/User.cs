#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace logregDemo1.Models;

public class User
{
    [Key]

    [Required(ErrorMessage = "UserID is required")]
    public int UserId { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "First Name must be at least 2 characters")]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
    public string Password { get; set; }

    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
