using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Filters;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository;

internal class TaskDatabaseService : ITaskDatabaseService
{
    private readonly TodoListDbContext context;
    private readonly IMapper mapper;

    public TaskDatabaseService(TodoListDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task CreateAsync(TaskModel model)
    {
        var entity = this.mapper.Map<TaskEntity>(model);
        entity.DateCreated = DateTime.Now;
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

    public async Task<IEnumerable<TaskModel>> GetAllAsync(TaskFilter filter)
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

        if (!string.IsNullOrEmpty(filter.TextInTitle))
        {
            tasks = tasks.Where(t => t.Title.Contains(filter.TextInTitle));
        }

        return await tasks.Select(x => this.mapper.Map<TaskModel>(x)).ToListAsync();
    }

    public async Task<IEnumerable<TaskModel>> GetAllAsync()
    {
        var tasks = await this.context.Tasks.ToListAsync();
        return tasks.Select(x => this.mapper.Map<TaskModel>(x));
    }

    public async Task<TaskModel?> GetByIdAsync(int id)
    {
        var task = await this.context.Tasks.FindAsync(id);
        return task is not null ? this.mapper.Map<TaskModel>(task) : null;
    }

    public async Task<bool> UpdateAsync(TaskModel model, int id)
    {
        var exist = await this.context.Tasks.FindAsync(id);
        if (exist != null)
        {
            exist.Title = model.Title;
            exist.Description = model.Description;
            exist.DueDate = model.DueDate;
            exist.Status = model.Status;
            exist.AssigneeId = model.AssigneeId;
            _ = await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> TodoListExist(int id)
    {
        var exist = await this.context.TodoLists.FindAsync(id);
        return exist != null;
    }
}
