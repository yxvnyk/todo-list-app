using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Filters;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository;

public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly TodoListDbContext context;
    private readonly IMapper mapper;

    public TodoListDatabaseService(TodoListDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<TodoListModel?> GetByIdAsync(int id)
    {
        var entity = await this.context.TodoLists!.FirstOrDefaultAsync(x => x.Id == id);
        return entity is null ? null : this.mapper.Map<TodoListModel>(entity);
    }

    public async Task<IEnumerable<TodoListModel>> GetAllAsync(TodoListFilter filter)
    {
        var entityList = this.context.TodoLists.AsQueryable();

        if (!string.IsNullOrEmpty(filter?.Title))
        {
            entityList = entityList.Where(t => t.Title == filter.Title);
        }

        if (filter?.UserId != null)
        {
            entityList = entityList.Where(t => t.UserId == filter.UserId);
        }

        return await entityList.Select(x => this.mapper.Map<TodoListModel>(x)).ToListAsync();
    }

    public async Task CreateAsync(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var entity = this.mapper.Map<TodoListEntity>(model);
        _ = this.context.Add(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(TodoListModel model, int id)
    {
        var entity = await this.context.TodoLists.FindAsync(id);
        if (entity != null)
        {
            entity.Description = model?.Description;
            entity.Title = model!.Title;
            _ = await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await this.context.TodoLists.FindAsync(id);
        if (entity != null)
        {
            _ = this.context.TodoLists.Remove(entity);
            _ = await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
