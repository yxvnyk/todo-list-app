using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.WebApi.Data;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions options)
        : base(options)
    {

    }

    public DbSet<TodoListEntity> TodoLists { get; set; }

    public DbSet<TaskEntity> Tasks { get; set; }

    public DbSet<CommentEntity> Comments { get; set; }

    public DbSet<TagEntity> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        _ = modelBuilder.Entity<TodoListEntity>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        _ = modelBuilder.Entity<TaskEntity>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
        _ = modelBuilder.Entity<CommentEntity>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
        _ = modelBuilder.Entity<TagEntity>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();
    }
}
