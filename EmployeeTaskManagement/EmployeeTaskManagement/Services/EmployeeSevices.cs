using EmployeeTaskManagement.Model;
using EmployeeTaskManagement.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Services;

public interface IEmployeeServices
{
    Task<ResponseModel> UpdateTaskStatus(string status, int taskId);
    Task<IEnumerable<EmployeeTasks>> GetAllTasks(string employeeId);
}

public class EmployeeServices : IEmployeeServices
{
    private EmployeeTaskManagementDbContext _context;
    public EmployeeServices(EmployeeTaskManagementDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseModel> UpdateTaskStatus(string status, int taskId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);

        if (task != null)
        {
            task.Status = status;
            await _context.SaveChangesAsync();
            return new ResponseModel { IsSuccess = true, Message = $"{task.Title} updated" };
        }

        return new ResponseModel { IsSuccess = false, Message = "Task not found" };
    }

    public async Task<IEnumerable<EmployeeTasks>> GetAllTasks(string currentUserId)
    {
        var allTasks = await (from task in _context.Tasks
                              join employee in _context.Employee
                              on task.EmployeeId equals employee.EmployeeId
                              where task.AssignedFrom == currentUserId
                              select new EmployeeTasks
                              {
                                  TaskId = task.TaskId,
                                  Title = task.Title,
                                  Description = task.Description,
                                  Status = task.Status,
                                  AssignedFrom = task.AssignedFrom,
                                  CreatedDate = task.CreatedDate,
                                  IsActive = task.IsActive,
                                  EmployeeId = task.EmployeeId,
                                  Employee = new Employee
                                  {
                                      Email = employee.Email
                                  }
                              }).ToListAsync();

        return allTasks;
    }

}
