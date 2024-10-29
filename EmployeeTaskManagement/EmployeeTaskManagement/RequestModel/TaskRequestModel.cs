using EmployeeTaskManagement.Model;

namespace EmployeeTaskManagement.RequestModel;

public class TaskRequestModel
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; }

    public string Email { get; set; }
}
