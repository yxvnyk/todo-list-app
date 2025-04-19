using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Helpers.Filters;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository;

public class CommentDatabaseService : ICommentDatabaseService
{
    private readonly TodoListDbContext context;
    private readonly IMapper mapper;

    public CommentDatabaseService(TodoListDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task CreateAsync(CommentDTO model)
    {
        var entity = this.mapper.Map<CommentEntity>(model);
        _ = await this.context.Comments.AddAsync(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var exist = await this.context.Comments.FindAsync(id);
        if (exist != null)
        {
            _ = this.context.Comments.Remove(exist);
            _ = await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<CommentDTO>> GetAllAsync(CommentFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var comments = this.context.Comments.AsQueryable();
        if (filter.TaskId > 0)
        {
            comments = comments.Where(c => c.TaskId == filter.TaskId);
        }

        var pageNumber = (filter.PageNumber - 1) * filter.PageSize;

        return await comments.Skip(pageNumber).Take(filter.PageSize).Select(x =>
            this.mapper.Map<CommentDTO>(x)).ToListAsync();
    }

    public async Task<CommentDTO?> GetByIdAsync(int id)
    {
        var exist = await this.context.Comments.FindAsync(id);
        return exist != null ? this.mapper.Map<CommentDTO>(exist) : null;
    }

    public async Task<bool> UpdateAsync(CommentUpdateDTO model, int id)
    {
        ArgumentNullException.ThrowIfNull(model);
        var exist = await this.context.Comments.FindAsync(id);
        if (exist != null)
        {
            _ = this.mapper.Map(model, exist);
            _ = await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> TaskExist(int id)
    {
        var exist = await this.context.Tasks.FindAsync(id);
        return exist != null;
    }
}
