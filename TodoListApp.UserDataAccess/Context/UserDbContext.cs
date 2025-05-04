using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models.Enums;

namespace TodoListApp.UserDataAccess.Context;
public class UserDbContext : IdentityDbContext<AppUser>
{
    public UserDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var roles = Enum.GetValues<RoleTypes>().Select(role => new IdentityRole
        {
            Name = role.ToString(),
            NormalizedName = role.ToString().ToUpper(),
        });
        _ = builder.Entity<IdentityRole>().HasData(roles);
    }
}
