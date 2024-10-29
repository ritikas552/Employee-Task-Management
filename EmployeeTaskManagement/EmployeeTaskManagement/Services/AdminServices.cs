using Azure.Core;
using EmployeeTaskManagement.Model;
using EmployeeTaskManagement.RequestModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeTaskManagement.Services;

public interface IAdminServices
{
    Task<ResponseModel> AddEmployee(EmployeeRequest request);
    Task<ResponseModel> UpdateEmployee(string email, EmployeeRequest request);
    Task<ResponseModel> DeleteEmployeeAsync(string email);
    Task<IEnumerable<Employee>> GetAllEmployees();
    ResponseModel CreateTask(TaskRequestModel request);
}

public class AdminServices : IAdminServices
{
    private EmployeeTaskManagementDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly LoggedUserService _loggedUserService;

    public AdminServices(EmployeeTaskManagementDbContext context, UserManager<User> userManager, LoggedUserService loggedUserService)
    {
        _context = context;
        _userManager = userManager;
        _loggedUserService = loggedUserService;
    }

    public async Task<ResponseModel> AddEmployee(EmployeeRequest request)
    {
        try
        {
            var existingEmployee = await _context.Employee.FirstOrDefaultAsync(e => e.Email == request.Email);
            if (existingEmployee != null)
            {
                return new ResponseModel
                {
                    Message = $"{existingEmployee.Email} already exists",
                    IsSuccess = false
                };
            }

            var isAddedUser = await AddEmployeeToIdentity(request);

            if (!isAddedUser.IsSuccess)
            {
                return isAddedUser;
            }


            var employeeModel = new Employee
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsActive = true,
            };

            await _context.Employee.AddAsync(employeeModel);
            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Message = $"{employeeModel.Email} created successfully",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel
            {
                Message = $"Error adding employee: {ex.Message}",
                IsSuccess = false
            };
        }
    }

    public async Task<ResponseModel> UpdateEmployee(string email, EmployeeRequest request)
    {
        try
        {
            var existing = await _context.Employee.FirstOrDefaultAsync(e => e.Email == email);
            if (existing == null)
            {
                return new ResponseModel { Message = $"{email} not found", IsSuccess = false };
            }

            existing.FirstName = request.FirstName;
            existing.LastName = request.LastName;

            _context.Employee.Update(existing);
            await _context.SaveChangesAsync();

            return new ResponseModel { Message = $"{existing.Email} updated successfully", IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseModel { Message = $"Error updating employee: {ex.Message}", IsSuccess = false };
        }
    }

    public async Task<ResponseModel> DeleteEmployeeAsync(string email)
    {
        try
        {
            var existing = await _context.Employee.FirstOrDefaultAsync(e => e.Email == email);
            if (existing == null)
            {
                return new ResponseModel { Message = $"{email} not exist", IsSuccess = false };
            }
            existing.IsActive = false;
            _context.Employee.Update(existing);
            await _context.SaveChangesAsync();

            return new ResponseModel { Message = $"{existing.Email} updated successfully", IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ResponseModel { Message = $"Error updating employee: {ex.Message}", IsSuccess = false };
        }
    }

    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        try
        {
            return await _context.Employee.Where(e => e.IsActive).ToListAsync();
        }
        catch (Exception ex)
        {
            return Enumerable.Empty<Employee>();
        }
    }

    public ResponseModel CreateTask(TaskRequestModel request)
    {
        var employee = _context.Employee.FirstOrDefault(e => e.Email == request.Email);
        if(employee == null)
        {
            return new ResponseModel()
            {
                Message = $"{employee} not found",
                IsSuccess = false
            };
        }
        var task = new EmployeeTasks
        {
            Title = request.Title,
            Description = request.Description,
            CreatedDate = DateTime.Now,
            IsActive = true,
            Status = request.Status,
            AssignedFrom = _loggedUserService.GetCurrentUser(),
            EmployeeId = employee.EmployeeId
        };
        try
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return new ResponseModel
            {
                Message = $"{request.Title} created successfully",
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel
            {
                Message = $"Error creating task: {ex.Message}",
                IsSuccess = false
            };
        }
    }

    private async Task<ResponseModel> AddEmployeeToIdentity(EmployeeRequest request)
    {
        var identityUser = new User
        {
            UserName = request.Email,
            Email = request.Email
        };
        var userCreationResult = await _userManager.CreateAsync(identityUser, request.Password);
        if (!userCreationResult.Succeeded)
        {
            var errors = string.Join(", ", userCreationResult.Errors.Select(e => e.Description));
            return new ResponseModel
            {
                Message = $"Error creating user: {errors}",
                IsSuccess = false
            };
        }

        var roleAssignmentResult = await _userManager.AddToRoleAsync(identityUser, "Employee");
        if (!roleAssignmentResult.Succeeded)
        {
            var errors = string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description));
            return new ResponseModel
            {
                Message = $"Error assigning role: {errors}",
                IsSuccess = false
            };
        }

        return new ResponseModel
        {
            IsSuccess = true
        };
    }
}
