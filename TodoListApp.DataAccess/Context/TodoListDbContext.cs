using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.DataAccess.Context;

/// <summary>
/// Represents the Entity Framework database context for the Todo List application.
/// </summary>
public class TodoListDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDbContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public TodoListDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TodoListEntity}"/> representing the Todo Lists table.
    /// </summary>
    public DbSet<TodoListEntity> TodoLists { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TaskEntity}"/> representing the Tasks table.
    /// </summary>
    public DbSet<TaskEntity> Tasks { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{CommentEntity}"/> representing the Comments table.
    /// </summary>
    public DbSet<CommentEntity> Comments { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TagEntity}"/> representing the Tags table.
    /// </summary>
    public DbSet<TagEntity> Tags { get; set; }

    /// <summary>
    /// Configures the model relationships and schema needed for the context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
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

        // Cascade deleting
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
