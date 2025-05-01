using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Helpers.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

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

    public async Task CreateAsync(TaskDTO model)
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

    public async Task<TaskPaging> GetAllAsync(TaskFilter filter)
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

        if (filter.Overdue == Models.Enums.Overdue.Active)
        {
            tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value > DateTime.Now);
        }
        else if (filter.Overdue == Models.Enums.Overdue.Overdue)
        {
            tasks = tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value < DateTime.Now);
        }

        if (!string.IsNullOrEmpty(filter.TextInTitle))
        {
            tasks = tasks.Where(t => t.Title.Contains(filter.TextInTitle, StringComparison.OrdinalIgnoreCase));
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

        var count = tasks.Count();

        return new TaskPaging()
        {
            Items = await tasks.Skip(pageNumber).Take(filter.PageSize).Select(x => this.mapper.Map<TaskDTO>(x)).ToListAsync(),
            TotalCount = (count + (filter.PageSize - 1)) / 5,
            CurrentPage = filter.PageNumber,
        };
    }

    public async Task<IEnumerable<TaskDTO>> GetAllAsync()
    {
        var tasks = await this.context.Tasks.ToListAsync();
        return tasks.Select(x => this.mapper.Map<TaskDTO>(x));
    }

    public async Task<TaskDTO?> GetByIdAsync(int id)
    {
        var task = await this.context.Tasks.FindAsync(id);
        return task is not null ? this.mapper.Map<TaskDTO>(task) : null;
    }

    public async Task<bool> UpdateAsync(TaskUpdateDTO model, int id)
    {
        var exist = await this.context.Tasks.FindAsync(id);
        if (exist != null)
        {
            _ = this.mapper.Map(model, exist);
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

    Task<IEnumerable<TaskDTO>> ICrud<TaskDTO, TaskUpdateDTO, TaskFilter>.GetAllAsync(TaskFilter filter)
    {
        throw new NotImplementedException();
    }
}
