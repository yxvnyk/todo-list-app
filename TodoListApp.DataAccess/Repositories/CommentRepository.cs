using TodoListApp.DataAccess.Context;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.DataAccess.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly TodoListDbContext context;

    public CommentRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(CommentEntity entity)
    {
        _ = await this.context.Comments.AddAsync(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var exist = await this.context.Comments.FindAsync(id);
        if (exist != null)
        {
            _ = this.context.Comments.Remove(exist);
            _ = await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public IQueryable<CommentEntity> GetAllAsync(CommentFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var comments = this.context.Comments.AsQueryable();
        if (filter?.TaskId > 0)
        {
            comments = comments.Where(c => c.TaskId == filter.TaskId);
        }

        int pageNumber = (filter!.PageNumber - 1) * filter.PageSize;

        return comments.Skip(pageNumber).Take(filter.PageSize);
    }

    public async Task<CommentEntity?> GetByIdAsync(int id)
    {
        var exist = await this.context.Comments.FindAsync(id);
        return exist;
    }

    public async Task SaveChangesAsync()
    {
        _ = await this.context.SaveChangesAsync();
    }
}
