using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository;

public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly TodoListDbContext context;

    public TodoListDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task AddTodoList(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var entity = new TodoListEntity()
        {
            Details = model.Details,
        };
        _ = this.context.Add(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<TodoListModel?> GetById(int id)
    {
        var entity = await this.context.TodoLists.FirstOrDefaultAsync(x => x.Id == id);
        return entity == null ? null : new TodoListModel()
        {
            Id = entity.Id,
            Details = entity.Details,
        };
    }

    public async Task<IEnumerable<TodoListModel>> GetAllTodoList()
    {
        var entityList = await this.context.TodoLists.ToListAsync();
        var list = entityList.Select(x => new TodoListModel()
        {
            Id = x.Id,
            Details = x.Details,
        });
        return list;
    }

    public async Task<bool> DeleteByIdTodoList(int id)
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

    public async Task<bool> UpdateTodoList(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var entity = await this.context.TodoLists.FindAsync(model?.Id);
        if (entity != null)
        {
            entity.Details = model!.Details;
            _ = await this.context.SaveChangesAsync();
            return true;
        }
    }
}
