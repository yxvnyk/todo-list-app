using TodoListApp.UserDataAccess;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}
