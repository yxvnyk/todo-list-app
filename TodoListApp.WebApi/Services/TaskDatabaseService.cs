using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository;

internal class TaskDatabaseService : ITaskDatabaseService
{
    private readonly ITaskRepository repository;
    private readonly IMapper mapper;

    public TaskDatabaseService(ITaskRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task CreateAsync(TaskDTO model)
    {
        var entity = this.mapper.Map<TaskEntity>(model);
        entity.DateCreated = DateTime.Now;
        await this.repository.CreateAsync(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        return await this.repository.DeleteByIdAsync(id);
    }

    public async Task<TaskPaging> GetAllAsync(TaskFilter filter)
    {
        var pair = await this.repository.GetAllAsync(filter);
        var tasks = pair.Item1;
        return new TaskPaging()
        {
            Items = await tasks.Select(x => this.mapper.Map<TaskDTO>(x)).ToListAsync(),
            TotalCount = (pair.Item2 + (filter.PageSize - 1)) / 5,
            CurrentPage = filter.PageNumber,
        };
    }

    public async Task<TaskDTO?> GetByIdAsync(int id)
    {
        var task = await this.repository.GetByIdAsync(id);
        return task is not null ? this.mapper.Map<TaskDTO>(task) : null;
    }

    public async Task<bool> UpdateAsync(TaskUpdateDTO model, int id)
    {
        var exist = await this.repository.GetByIdAsync(id);
        if (exist != null)
        {
            _ = this.mapper.Map(model, exist);
            await this.repository.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> TaskExist(int id)
    {
        return await this.repository.TaskExist(id);
    }

    public string? GetTaskOwnerId(int taskId)
    {
        return this.repository.GetTaskOwnerId(taskId);
    }

    Task<IEnumerable<TaskDTO>> ICrud<TaskDTO, TaskUpdateDTO, TaskFilter>.GetAllAsync(TaskFilter filter)
    {
        throw new NotImplementedException();
    }
}
