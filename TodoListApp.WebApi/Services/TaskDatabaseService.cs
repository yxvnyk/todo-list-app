using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Repositories.Interfaces;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Data.Repository
{
    /// <summary>
    /// Service implementation for managing tasks in the database. Provides methods for creating, updating, retrieving, deleting, and checking tasks.
    /// </summary>
    internal class TaskDatabaseService : ITaskDatabaseService
    {
        private readonly ITaskRepository repository;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDatabaseService"/> class.
        /// </summary>
        /// <param name="repository">The repository for performing database operations on tasks.</param>
        /// <param name="mapper">The AutoMapper instance used for mapping between DTOs and entities.</param>
        public TaskDatabaseService(ITaskRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Creates a new task in the database.
        /// </summary>
        /// <param name="model">The task DTO containing the information to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateAsync(TaskDTO model)
        {
            var entity = this.mapper.Map<TaskEntity>(model);
            entity.DateCreated = DateTime.Now;
            await this.repository.CreateAsync(entity);
        }

        /// <summary>
        /// Deletes a task from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success or failure.</returns>
        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await this.repository.DeleteByIdAsync(id);
        }

        /// <summary>
        /// Retrieves all tasks that match the given filter, with pagination support.
        /// </summary>
        /// <param name="filter">The filter used to query the tasks.</param>
        /// <returns>A task representing the asynchronous operation, with a paginated result of task DTOs.</returns>
        public async Task<TaskPaging> GetAllAsync(TaskFilter filter)
        {
            var pair = await this.repository.GetAllAsync(filter);
            var tasks = pair.Item1;
            return new TaskPaging()
            {
                Items = await tasks.Select(x => this.mapper.Map<TaskDTO>(x)).ToListAsync(),
                TotalCount = (pair.Item2 + (filter.PageSize - 1)) / 5,
                CurrentPage = filter.PageNumber,
            };
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with the task DTO or null if not found.</returns>
        public async Task<TaskDTO?> GetByIdAsync(int id)
        {
            var task = await this.repository.GetByIdAsync(id);
            return task is not null ? this.mapper.Map<TaskDTO>(task) : null;
        }

        /// <summary>
        /// Updates an existing task with the provided data.
        /// </summary>
        /// <param name="model">The task update DTO containing the new data.</param>
        /// <param name="id">The ID of the task to update.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success or failure.</returns>
        public async Task<bool> UpdateAsync(TaskUpdateDTO model, int id)
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
        /// Checks if a task exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the task to check.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating if the task exists.</returns>
        public async Task<bool> TaskExist(int id)
        {
            return await this.repository.TaskExist(id);
        }

        /// <summary>
        /// Retrieves the owner ID of a task by its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The owner ID of the task, or null if not found.</returns>
        public string? GetTaskOwnerId(int taskId)
        {
            return this.repository.GetTaskOwnerId(taskId);
        }

        /// <summary>
        /// Retrieves all tasks that match the given filter.
        /// This method is part of the ICrud interface, but is not implemented here as pagination is handled in the custom GetAllAsync method.
        /// </summary>
        /// <param name="filter">The filter used to query the tasks.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of task DTOs.</returns>
        Task<IEnumerable<TaskDTO>> ICrud<TaskDTO, TaskUpdateDTO, TaskFilter>.GetAllAsync(TaskFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
