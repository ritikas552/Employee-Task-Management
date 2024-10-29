using EmployeeTaskManagement.Model;
using EmployeeTaskManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeTaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly LoggedUserService _loggedUserService;
        private readonly UserManager<User> _userManager;

        public EmployeeController(IEmployeeServices employeeServices, LoggedUserService loggedUserService, UserManager<User> userManager)
        {
            _employeeServices = employeeServices;
            _loggedUserService = loggedUserService;
            _userManager = userManager;
        }

        [HttpPost("UpdateTaskStatus")]
        public async Task<IActionResult> UpdateTaskStatus(string status, int taskId)
        {
            try
            {
                var response = await _employeeServices.UpdateTaskStatus(status, taskId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var currentUserId = _loggedUserService.GetCurrentUser();
                var response = await _employeeServices.GetAllTasks(currentUserId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
