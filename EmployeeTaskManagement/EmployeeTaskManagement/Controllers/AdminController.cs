using EmployeeTaskManagement.Model;
using EmployeeTaskManagement.RequestModel;
using EmployeeTaskManagement.Services;
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
        public IActionResult CreateEmployee(EmployeeRequest request)
        {
            try
            {
                var response = _adminServices.AddEmployee(request);
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("CreateTasks")]
        public IActionResult CreateTask(EmployeeRequest request)
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
