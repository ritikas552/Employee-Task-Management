using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Model;

public class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
