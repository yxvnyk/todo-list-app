using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.Context;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.DataAccess.Repositories;

public class TagRepository : ITagRepository
{
    private readonly TodoListDbContext context;

    public TagRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public IQueryable<TagEntity> GetAllAsync(TagFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);
        var tags = this.context.Tags.AsQueryable();
        if (filter.TaskId > 0)
        {
            tags = tags.Where(t => t.TaskId == filter.TaskId);
        }

        if (filter?.TodoListId > 0)
        {
            tags = tags.Include(t => t.Task).Where(t => t.Task != null && t.Task.TodoListId == filter.TodoListId);
        }

        if (!string.IsNullOrEmpty(filter?.AssigneeId) || !string.IsNullOrEmpty(filter?.OwnerId))
        {
            tags = tags
                .Include(t => t.Task)
                    .ThenInclude(task => task.TodoList)
                .Where(t =>
                    t.Task != null &&
                    (
                        (!string.IsNullOrEmpty(filter.AssigneeId) && t.Task.AssigneeId == filter.AssigneeId)
                        ||
                        (!string.IsNullOrEmpty(filter.OwnerId) && t.Task.TodoList != null && t.Task.TodoList.OwnerId == filter.OwnerId)));
        }

        var pageNumber = (filter!.PageNumber - 1) * filter.PageSize;

        return tags.Skip(pageNumber).Take(filter.PageSize);
    }

    public async Task CreateAsync(TagEntity entity)
    {
        _ = await this.context.Tags.AddAsync(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<TagEntity?> GetByIdAsync(int id)
    {
        var exist = await this.context.Tags.FindAsync(id);
        return exist;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var exist = await this.context.Tags.FindAsync(id);
        if (exist != null)
        {
            _ = this.context.Tags.Remove(exist);
            _ = await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
