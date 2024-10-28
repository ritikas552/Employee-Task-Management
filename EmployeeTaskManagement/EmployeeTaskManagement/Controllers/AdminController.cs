using EmployeeTaskManagement.Model;
using EmployeeTaskManagement.RequestModel;
using EmployeeTaskManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpPost("CreateEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmployee(EmployeeRequest request)
        {
            try
            {
                if(request.Password == string.Empty)
                {
                    request.Password = "Test@12345";
                };
                var response = await _adminServices.AddEmployee(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("UpdateEmployee/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(string email, EmployeeRequest request)
        {
            try
            {
                var response = await _adminServices.UpdateEmployee(email, request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteEmployee/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(string email)
        {
            try
            {
                var response = await _adminServices.DeleteEmployeeAsync(email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllEmployees")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var response = await _adminServices.GetAllEmployees();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateTasks")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateTask(TaskRequestModel request)
        {
            try
            {
                var response = _adminServices.CreateTask(request);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
