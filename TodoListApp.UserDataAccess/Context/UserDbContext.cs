using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListApp.UserDataAccess.Entity;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.UserDataAccess.Context;

/// <summary>
/// Represents the database context for user-related data, extending the IdentityDbContext to manage user authentication and authorization.
/// </summary>
public class UserDbContext : IdentityDbContext<AppUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDbContext"/> class with the specified options.
    /// </summary>
    /// <param name="dbContextOptions">The options to configure the database context.</param>
    public UserDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }

    /// <summary>
    /// Configures the model for the user-related entities, including seeding the roles.
    /// </summary>
    /// <param name="builder">The model builder used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed roles from the RoleTypes enum into the IdentityRole table
        var roles = Enum.GetValues<RoleTypes>().Select(role => new IdentityRole
        {
            Name = role.ToString(),
            NormalizedName = role.ToString().ToUpper(),
        });

        _ = builder.Entity<IdentityRole>().HasData(roles);
    }
}
