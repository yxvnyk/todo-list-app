using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.Context;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.DataAccess.Repositories;

public class TodoListRepository : ITodoListRepository
{
    private readonly TodoListDbContext context;

    public TodoListRepository(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<TodoListEntity?> GetByIdAsync(int id)
    {
        var entity = await this.context.TodoLists.FirstOrDefaultAsync(x => x.Id == id);
        return entity;
    }

    public IQueryable<TodoListEntity> GetAllAsync(TodoListFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var entityList = this.context.TodoLists.AsQueryable();

        if (!string.IsNullOrEmpty(filter.Title))
        {
            entityList = entityList.Where(t => t.Title == filter.Title);
        }

        if (filter.OwnerId != null)
        {
            entityList = entityList.Where(t => t.OwnerId == filter.OwnerId);
        }

        var pageNumber = (filter.PageNumber - 1) * filter.PageSize;

        return entityList.Skip(pageNumber).Take(filter.PageSize);
    }

    public async Task CreateAsync(TodoListEntity entity)
    {
        _ = this.context.Add(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        _ = await this.context.SaveChangesAsync();
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

    public async Task<bool> TodoListExist(int id)
    {
        var exist = await this.context.TodoLists.FindAsync(id);
        return exist != null;
    }
}
