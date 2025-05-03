using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.Context;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.DataAccess.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TodoListDbContext context;

    public TaskRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(TaskEntity entity)
    {
        _ = await this.context.Tasks.AddAsync(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var exist = await this.context.Tasks.FindAsync(id);
        if (exist != null)
        {
            _ = this.context.Tasks.Remove(exist);
            _ = await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<(IQueryable<TaskEntity>, int)> GetAllAsync(TaskFilter filter)
    {
        var tasks = this.context.Tasks.AsQueryable();

        if (!string.IsNullOrEmpty(filter.AssigneeId))
        {
            tasks = tasks.Where(t => t.AssigneeId == filter.AssigneeId);
        }

        if (filter.TodoListId > 0)
        {
            tasks = tasks.Where(t => t.TodoListId == filter.TodoListId);
        }

        if (filter.Status != null)
        {
            tasks = tasks.Where(t => t.Status == filter.Status);
        }

        if (filter.TagName != null)
        {
            tasks = tasks.Include(t => t.Tags).Where(t => t.Tags != null && t.Tags.Any(tag => tag.Name == filter.TagName));
        }

        if (filter.Overdue == Filters.Enums.Overdue.Active)
        {
            tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value > DateTime.Now);
        }
        else if (filter.Overdue == Filters.Enums.Overdue.Overdue)
        {
            tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value < DateTime.Now);
        }

        if (!string.IsNullOrEmpty(filter.TextInTitle))
        {
            var pattern = $"%{filter.TextInTitle}%";
            tasks = tasks.Where(t => EF.Functions.Like(t.Title, pattern));
        }

        if (filter.DueDate != null)
        {
            var from = filter.DueDate.Value.Date;
            var to = from.AddDays(1);

            tasks = tasks.Where(t => t.DueDate >= from && t.DueDate < to);
        }

        if (filter.CreationDate != null)
        {
            var from = filter.CreationDate.Value.Date;
            var to = from.AddDays(1);

            tasks = tasks.Where(t => t.DateCreated >= from && t.DateCreated < to);
        }

        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            if (filter.SortBy.Equals("DueDate", StringComparison.OrdinalIgnoreCase))
            {
                tasks = filter.IsDescending ? tasks.OrderByDescending(x => x.DueDate) : tasks.OrderBy(x => x.DueDate);
            }
            else if (filter.SortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
            {
                tasks = filter.IsDescending ? tasks.OrderByDescending(x => x.Title) : tasks.OrderBy(x => x.Title);
            }
        }

        var pageNumber = (filter.PageNumber - 1) * filter.PageSize;
        var count = await tasks.CountAsync();
        return (tasks.Skip(pageNumber).Take(filter.PageSize), count);
    }

    public async Task<TaskEntity?> GetByIdAsync(int id)
    {
        var task = await this.context.Tasks.FindAsync(id);
        return task;
    }

    public async Task SaveChangesAsync()
    {
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<bool> TaskExist(int id)
    {
        var exist = await this.context.Tasks.FindAsync(id);
        return exist != null;
    }

    IQueryable<TaskEntity> ICrud<TaskEntity, TaskFilter>.GetAllAsync(TaskFilter filter)
    {
        throw new NotImplementedException();
    }
}
