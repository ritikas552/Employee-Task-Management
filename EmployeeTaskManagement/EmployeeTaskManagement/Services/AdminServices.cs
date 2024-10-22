using EmployeeTaskManagement.Model;
using EmployeeTaskManagement.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Services;

public interface IAdminServices
{
    ResponseModel AddEmployee(EmployeeRequest request);
    ResponseModel CreateTask(EmployeeRequest request);
}

public class AdminServices : IAdminServices
{
    private EmployeeTaskManagementDbContext _context;

    public AdminServices(EmployeeTaskManagementDbContext context)
    {
        _context = context;
    }

    public ResponseModel AddEmployee(EmployeeRequest request)
    {
        var employeeModel = new Employee { Email = request.Email, FirstName = request.FirstName, LastName = request.LastName };
        var existing = _context.Employee.FirstOrDefault(e => e.Email == employeeModel.Email);
        if (existing != null)
        {
            return new ResponseModel { ErrorMessage = $"{existing.Email} is already Exist" };
        }

        _context.Employee.Add(employeeModel);
        _context.SaveChanges();
        return new ResponseModel { ErrorMessage = $"{employeeModel.Email} is already Exist", IsSuccess = true };
    }

    public ResponseModel CreateTask(EmployeeRequest request)
    {
        var employeeModel = new Employee { Email = request.Email, FirstName = request.FirstName, LastName = request.LastName };
        var existing = _context.Tasks.FirstOrDefault(e => e.Email == employeeModel.Email);
        if (existing != null)
        {
            return new ResponseModel { ErrorMessage = $"{existing.Email} is already Exist" };
        }

        _context.Employee.Add(employeeModel);
        _context.SaveChanges();
        return new ResponseModel { ErrorMessage = $"{employeeModel.Email} is already Exist", IsSuccess = true };
    }
}
