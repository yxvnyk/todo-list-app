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
    /// Service implementation for managing TodoLists in the database. Provides methods for creating, updating, retrieving, deleting, and checking TodoLists.
    /// </summary>
    public class TodoListDatabaseService : ITodoListDatabaseService
    {
        private readonly ITodoListRepository repository;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListDatabaseService"/> class.
        /// </summary>
        /// <param name="repository">The repository for performing database operations on TodoLists.</param>
        /// <param name="mapper">The AutoMapper instance used for mapping between DTOs and entities.</param>
        public TodoListDatabaseService(ITodoListRepository context, IMapper mapper)
        {
            this.repository = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Retrieves a TodoList by its ID.
        /// </summary>
        /// <param name="id">The ID of the TodoList to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with the TodoList DTO or null if not found.</returns>
        public async Task<TodoListDto?> GetByIdAsync(int id)
        {
            var entity = await this.repository.GetByIdAsync(id);
            return entity is null ? null : this.mapper.Map<TodoListDto>(entity);
        }

        /// <summary>
        /// Retrieves all TodoLists that match the given filter.
        /// </summary>
        /// <param name="filter">The filter used to query the TodoLists.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of TodoList DTOs.</returns>
        public async Task<IEnumerable<TodoListDto>> GetAllAsync(TodoListFilter filter)
        {
            var lists = this.repository.GetAllAsync(filter);
            return await lists.Select(x => this.mapper.Map<TodoListDto>(x)).ToListAsync();
        }

        /// <summary>
        /// Creates a new TodoList in the database.
        /// </summary>
        /// <param name="model">The TodoList DTO containing the information to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateAsync(TodoListDto model)
        {
            var entity = this.mapper.Map<TodoListEntity>(model);
            await this.repository.CreateAsync(entity);
        }

        /// <summary>
        /// Updates an existing TodoList in the database with the provided update data.
        /// </summary>
        /// <param name="model">The TodoList update DTO containing the new data.</param>
        /// <param name="id">The ID of the TodoList to update.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success or failure.</returns>
        public async Task<bool> UpdateAsync(TodoListUpdateDto model, int id)
        {
            var entity = await this.repository.GetByIdAsync(id);
            if (entity != null)
            {
                _ = this.mapper.Map(model, entity);
                await this.repository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes a TodoList from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the TodoList to delete.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success or failure.</returns>
        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await this.repository.DeleteByIdAsync(id);
        }

        /// <summary>
        /// Checks if a TodoList exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the TodoList to check.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating if the TodoList exists.</returns>
        public Task<bool> TodoListExist(int id)
        {
            return this.repository.TodoListExist(id);
        }
    }
}
