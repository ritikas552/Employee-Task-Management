using EmployeeTaskManagement.Model;

namespace EmployeeTaskManagement.Services;

public interface ITokenBlacklistService
{
    Task AddToBlacklistAsync(string token);
    Task<bool> IsTokenBlacklistedAsync(string token);
}
public class TokenBlackListService : ITokenBlacklistService
{
    private readonly List<BlackListedToken> _blacklistedTokens = new();

    public async Task AddToBlacklistAsync(string token)
    {
        _blacklistedTokens.Add(new BlackListedToken { Token = token, ExpiryDate = DateTime.UtcNow });
        await Task.CompletedTask;
    }

    public async Task<bool> IsTokenBlacklistedAsync(string token)
    {
        return await Task.FromResult(_blacklistedTokens.Any(t => t.Token == token && t.ExpiryDate > DateTime.Now));
    }
}
