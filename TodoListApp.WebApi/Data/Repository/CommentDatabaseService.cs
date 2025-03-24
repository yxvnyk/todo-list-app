using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
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

    public async Task Create(CommentModel model)
    {
        var entity = this.mapper.Map<CommentEntity>(model);
        _ = await this.context.Comments.AddAsync(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<bool> DeleteById(int id)
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

    public async Task<IEnumerable<CommentModel>> GetAll()
    {
        var tags = await this.context.Comments.ToListAsync();
        return tags.Select(x =>
            this.mapper.Map<CommentModel>(x));
    }

    public async Task<CommentModel?> GetById(int id)
    {
        var exist = await this.context.Comments.FindAsync(id);
        return exist != null ? this.mapper.Map<CommentModel>(exist) : null;
    }

    public async Task<bool> Update(CommentModel model, int id)
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
