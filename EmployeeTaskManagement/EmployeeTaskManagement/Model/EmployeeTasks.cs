using System.ComponentModel.DataAnnotations;

namespace EmployeeTaskManagement.Model;

public class EmployeeTasks : baseEntity
{
    [Key]
    public int TaskId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; }

    public string AssignedFrom { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; }

    public string? AssignedTo => Employee?.Email;
}

