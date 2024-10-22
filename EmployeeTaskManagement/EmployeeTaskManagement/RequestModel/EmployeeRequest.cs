using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.RequestModel;

public class EmployeeRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
