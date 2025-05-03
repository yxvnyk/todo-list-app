using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.DataAccess.Context;

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

        // cascade deleting
        _ = modelBuilder.Entity<TodoListEntity>()
            .HasMany(tl => tl.Tasks)
            .WithOne(t => t.TodoList)
            .HasForeignKey(t => t.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<TaskEntity>()
         .HasMany(t => t.Comments)
         .WithOne(c => c.Task)
         .HasForeignKey(c => c.TaskId)
         .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<TaskEntity>()
         .HasMany(t => t.Tags)
         .WithOne(c => c.Task)
         .HasForeignKey(c => c.TaskId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}
