using EmployeeTaskManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EmployeeTaskManagement.Services;

public class LoggedUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;

    public LoggedUserService(UserManager<User> userManager, IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
    }

    public string GetCurrentUser()
    {
        var claims = _contextAccessor?.HttpContext?.User.Claims;

        return claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
    }
}
