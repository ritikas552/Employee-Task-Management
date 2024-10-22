using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Model;

public class Employee
{
    [Key]
    public Guid EmployeeId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public ICollection<EmployeeTasks> EmployeeTasks { get; set; }
}
