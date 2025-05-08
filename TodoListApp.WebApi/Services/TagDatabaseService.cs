using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository
{
    /// <summary>
    /// Service implementation for managing tags in the database. Provides methods for creating, updating, retrieving, and deleting tags.
    /// </summary>
    internal class TagDatabaseService : ITagDatabaseService
    {
        private readonly ITagRepository repository;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagDatabaseService"/> class.
        /// </summary>
        /// <param name="context">The repository for performing database operations on tags.</param>
        /// <param name="mapper">The AutoMapper instance used for mapping between DTOs and entities.</param>
        public TagDatabaseService(ITagRepository context, IMapper mapper)
        {
            this.repository = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Retrieves all tags that match the given filter.
        /// </summary>
        /// <param name="filter">The filter used to query the tags.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of tag DTOs.</returns>
        public async Task<IEnumerable<TagDTO>> GetAllAsync(TagFilter filter)
        {
            var tags = await this.repository.GetAllAsync(filter).Select(x =>
                this.mapper.Map<TagDTO>(x)).ToListAsync();
            return tags;
        }

        /// <summary>
        /// Creates a new tag in the database.
        /// </summary>
        /// <param name="model">The tag DTO containing the information to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateAsync(TagDTO model)
        {
            var entity = this.mapper.Map<TagEntity>(model);
            await this.repository.CreateAsync(entity);
        }

        /// <summary>
        /// Updates an existing tag with the provided data.
        /// </summary>
        /// <param name="model">The tag update DTO containing the new data.</param>
        /// <param name="id">The ID of the tag to update.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success or failure.</returns>
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

        /// <summary>
        /// Retrieves a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with the tag DTO or null if not found.</returns>
        public async Task<TagDTO?> GetByIdAsync(int id)
        {
            var exist = await this.repository.GetByIdAsync(id);
            return exist != null ? this.mapper.Map<TagDTO>(exist) : null;
        }

        /// <summary>
        /// Deletes a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success or failure.</returns>
        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await this.repository.DeleteByIdAsync(id);
        }
    }
}
