using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Data.Repository;

internal class TagDatabaseService : ITagDatabaseService
{
    private readonly TodoListDbContext context;
    private readonly IMapper mapper;

    public TagDatabaseService(TodoListDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<TagModel>> GetAllAsync()
    {
        var tags = await this.context.Tags.ToListAsync();
        return tags.Select(x =>
            this.mapper.Map<TagModel>(x));
    }

    public async Task CreateAsync(TagModel model)
    {
        var entity = this.mapper.Map<TagEntity>(model);
        _ = await this.context.Tags.AddAsync(entity);
        _ = await this.context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(TagModel model, int id)
    {
        var exist = await this.context.Tags.FindAsync(id);
        if (exist != null)
        {
            exist.Name = model.Name;
            _ = await this.context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<TagModel?> GetByIdAsync(int id)
    {
        var exist = await this.context.Tags.FindAsync(id);
        return exist != null ? this.mapper.Map<TagModel>(exist) : null;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var exist = await this.context.Tags.FindAsync(id);
        if (exist != null)
        {
            _ = this.context.Tags.Remove(exist);
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
