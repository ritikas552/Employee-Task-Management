using EmployeeTaskManagement.Model;
using EmployeeTaskManagement.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Services;

public interface IEmployeeServices
{
    Task<ResponseModel> UpdateTaskStatus(string status, int taskId);
    Task<IEnumerable<EmployeeTasks>> GetAllTasks(Guid employeeId);
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

    public async Task<IEnumerable<EmployeeTasks>> GetAllTasks(Guid employeeId)
    {
        return await _context.Tasks.Where(t => t.EmployeeId == employeeId).ToListAsync();
    }

}
