#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace logregDemo1.Models;

public class LogUser
{
    [Required]
    [EmailAddress]
    public string LogEmail { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
    public string LogPassword { get; set; }
}
