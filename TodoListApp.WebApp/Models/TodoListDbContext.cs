using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace TodoListApp.WebApp.Models;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions options)
        : base(options)
    {

    }
}
