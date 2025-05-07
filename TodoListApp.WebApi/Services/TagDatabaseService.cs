using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository;

internal class TagDatabaseService : ITagDatabaseService
{
    private readonly ITagRepository repository;
    private readonly IMapper mapper;

    public TagDatabaseService(ITagRepository context, IMapper mapper)
    {
        this.repository = context;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<TagDTO>> GetAllAsync(TagFilter filter)
    {
        var tags = await this.repository.GetAllAsync(filter).Select(x =>
            this.mapper.Map<TagDTO>(x)).ToListAsync();
        return tags;
    }

    public async Task CreateAsync(TagDTO model)
    {
        var entity = this.mapper.Map<TagEntity>(model);
        await this.repository.CreateAsync(entity);
    }

    public async Task<bool> UpdateAsync(TagUpdateDTO model, int id)
    {
        var exist = await this.repository.GetByIdAsync(id);
        if (exist != null)
        {
            _ = this.mapper.Map(model, exist);
            await this.repository.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<TagDTO?> GetByIdAsync(int id)
    {
        var exist = await this.repository.GetByIdAsync(id);
        return exist != null ? this.mapper.Map<TagDTO>(exist) : null;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        return await this.repository.DeleteByIdAsync(id);
    }
}
