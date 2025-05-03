using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository;

public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly ITodoListRepository repository;
    private readonly IMapper mapper;

    public TodoListDatabaseService(ITodoListRepository context, IMapper mapper)
    {
        this.repository = context;
        this.mapper = mapper;
    }

    public async Task<TodoListDTO?> GetByIdAsync(int id)
    {
        var entity = await this.repository.GetByIdAsync(id);
        return entity is null ? null : this.mapper.Map<TodoListDTO>(entity);
    }

    public async Task<IEnumerable<TodoListDTO>> GetAllAsync(TodoListFilter filter)
    {
        var lists = this.repository.GetAllAsync(filter);
        return await lists.Select(x => this.mapper.Map<TodoListDTO>(x)).ToListAsync();
    }

    public async Task CreateAsync(TodoListDTO model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var entity = this.mapper.Map<TodoListEntity>(model);
        await this.repository.CreateAsync(entity);
    }

    public async Task<bool> UpdateAsync(TodoListUpdateDTO model, int id)
    {
        var entity = await this.repository.GetByIdAsync(id);
        if (entity != null)
        {
            _ = this.mapper.Map(model, entity);
            _ = this.repository.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        return await this.repository.DeleteByIdAsync(id);
    }
}
