using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
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

    public async Task Create(TaskModel model)
    {
        var entity = this.mapper.Map<TaskEntity>(model);
        _ = await this.context.Tasks.AddAsync(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<bool> DeleteById(int id)
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

    public async Task<IEnumerable<TaskModel>> GetAll()
    {
        var tasks = await this.context.Tasks.ToListAsync();
        return tasks.Select(x => this.mapper.Map<TaskModel>(x));
    }

    public async Task<TaskModel?> GetById(int id)
    {
        var task = await this.context.Tasks.FindAsync(id);
        return this.mapper.Map<TaskModel>(task);
    }

    public async Task<bool> Update(TaskModel model, int id)
    {
        var exist = await this.context.Tasks.FindAsync(id);
        if (exist != null)
        {
            exist.Title = model.Title;
            exist.DueDate = model.DueDate;
            exist.IsCompleted = model.IsCompleted;
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
