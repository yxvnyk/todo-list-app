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
    /// Service implementation for managing comments in the database. Provides methods for creating, updating, retrieving, and deleting comments.
    /// </summary>
    public class CommentDatabaseService : ICommentDatabaseService
    {
        private readonly ICommentRepository repository;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentDatabaseService"/> class.
        /// </summary>
        /// <param name="repository">The repository for performing database operations on comments.</param>
        /// <param name="mapper">The AutoMapper instance used for mapping between DTOs and entities.</param>
        public CommentDatabaseService(ICommentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Creates a new comment in the database.
        /// </summary>
        /// <param name="model">The comment DTO containing the information to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateAsync(CommentDto model)
        {
            var entity = this.mapper.Map<CommentEntity>(model);
            await this.repository.CreateAsync(entity);
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success or failure.</returns>
        public async Task<bool> DeleteByIdAsync(int id)
        {
            var result = await this.repository.DeleteByIdAsync(id);
            return result;
        }

        /// <summary>
        /// Retrieves all comments that match the given filter.
        /// </summary>
        /// <param name="filter">The filter used to query the comments.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of comment DTOs.</returns>
        public async Task<IEnumerable<CommentDto>> GetAllAsync(CommentFilter filter)
        {
            var comments = await this.repository.GetAllAsync(filter)
                .Select(x => this.mapper.Map<CommentDto>(x)).ToListAsync();

            return comments;
        }

        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with the comment DTO or null if not found.</returns>
        public async Task<CommentDto?> GetByIdAsync(int id)
        {
            var entity = await this.repository.GetByIdAsync(id);
            return entity != null ? this.mapper.Map<CommentDto>(entity) : null;
        }

        /// <summary>
        /// Checks if a task exists with the given ID. (Currently not implemented in this service.)
        /// </summary>
        /// <param name="id">The ID of the task to check.</param>
        /// <returns>A task representing the asynchronous operation. Throws a NotImplementedException.</returns>
        public Task<bool> TaskExist(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing comment with the provided data.
        /// </summary>
        /// <param name="model">The comment update DTO containing the new data.</param>
        /// <param name="id">The ID of the comment to update.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success or failure.</returns>
        public async Task<bool> UpdateAsync(CommentUpdateDto model, int id)
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
    }
}
