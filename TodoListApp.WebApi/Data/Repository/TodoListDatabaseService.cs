using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
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

    public async Task<TodoListModel?> GetById(int id)
    {
        var entity = await this.context.TodoLists!.FirstOrDefaultAsync(x => x.Id == id);
        return entity == null ? null : this.mapper.Map<TodoListModel>(entity);
        //return entity == null ? null : new TodoListModel()
        //{
        //    Id = entity.Id,
        //    Description = entity.Description,
        //};
    }

    public async Task<IEnumerable<TodoListModel>> GetAll()
    {
        var entityList = await this.context.TodoLists.ToListAsync();
        //var list = entityList.Select(x => new TodoListModel()
        //{
        //    Id = x.Id,
        //    Description = x.Description,
        //});
        var list = entityList.Select(x => this.mapper.Map<TodoListModel>(x));
        return list;
    }

    public async Task Create(TodoListModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        //var entity = new TodoListEntity()
        //{
        //    Description = model.Description,
        //};
        var entity = this.mapper.Map<TodoListEntity>(model);
        _ = this.context.Add(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<bool> Update(TodoListModel model, int id)
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

    public async Task<bool> DeleteById(int id)
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
