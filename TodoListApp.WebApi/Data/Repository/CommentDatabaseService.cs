using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Filters;
using TodoListApp.WebApi.Models;

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

    public async Task CreateAsync(CommentModel model)
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

    public async Task<IEnumerable<CommentModel>> GetAllAsync(CommentFilter filter)
    {
        var comments = this.context.Comments.AsQueryable();
        if (filter?.TaskId > 0)
        {
            comments = comments.Where(c => c.TaskId == filter.TaskId);
        }

        return await comments.Select(x =>
            this.mapper.Map<CommentModel>(x)).ToListAsync();
    }

    public async Task<CommentModel?> GetByIdAsync(int id)
    {
        var exist = await this.context.Comments.FindAsync(id);
        return exist != null ? this.mapper.Map<CommentModel>(exist) : null;
    }

    public async Task<bool> UpdateAsync(CommentModel model, int id)
    {
        ArgumentNullException.ThrowIfNull(model);
        var exist = await this.context.Comments.FindAsync(id);
        if (exist != null)
        {
            exist.Comment = model.Comment;
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
