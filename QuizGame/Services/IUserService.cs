using Microsoft.AspNetCore.Http;
using QuizGame.ViewModels;
using System.Security.Claims;

public interface IUserService
{
    UserInfo GetCurrentUser();
}

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public UserInfo GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity.IsAuthenticated)
            return null;

        return new UserInfo
        {
            UserId = user.FindFirstValue(ClaimTypes.NameIdentifier),
            FullName = user.Identity.Name,
            Email = user.FindFirstValue(ClaimTypes.Email)
        };
    }
}
