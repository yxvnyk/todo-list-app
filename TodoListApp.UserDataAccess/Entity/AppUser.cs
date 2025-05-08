using Microsoft.AspNetCore.Identity;

namespace TodoListApp.UserDataAccess.Entity
{
    /// <summary>
    /// Represents an application user, extending from <see cref="IdentityUser"/> to include authentication and authorization information.
    /// </summary>
    public class AppUser : IdentityUser
    {
    }
}
