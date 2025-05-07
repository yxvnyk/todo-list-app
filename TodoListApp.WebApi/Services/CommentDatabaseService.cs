using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository;

public class CommentDatabaseService : ICommentDatabaseService
{
    private readonly ICommentRepository repository;
    private readonly IMapper mapper;

    public CommentDatabaseService(ICommentRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task CreateAsync(CommentDTO model)
    {
        var entity = this.mapper.Map<CommentEntity>(model);
        await this.repository.CreateAsync(entity);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var result = await this.repository.DeleteByIdAsync(id);
        return result;
    }

    public async Task<IEnumerable<CommentDTO>> GetAllAsync(CommentFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var comments = await this.repository.GetAllAsync(filter).Select(x =>
            this.mapper.Map<CommentDTO>(x)).ToListAsync();

        return comments;
    }

    public async Task<CommentDTO?> GetByIdAsync(int id)
    {
        var entity = await this.repository.GetByIdAsync(id);
        return entity != null ? this.mapper.Map<CommentDTO>(entity) : null;
    }

    public Task<bool> TaskExist(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(CommentUpdateDTO model, int id)
    {
        ArgumentNullException.ThrowIfNull(model);
        var exist = await this.repository.GetByIdAsync(id);
        if (exist != null)
        {
            _ = this.mapper.Map(model, exist);
            await this.repository.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
