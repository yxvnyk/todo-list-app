using TodoListApp.UserDataAccess.Entity;

namespace TodoListApp.WebApp.Services.Interfaces
{
    /// <summary>
    /// Defines methods for creating authentication tokens for users.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token will be generated.</param>
        /// <returns>A task that represents the asynchronous operation, with the token as the result.</returns>
        Task<string> CreateToken(AppUser user);
    }
}
