using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models.Logging;

namespace TodoListApp.WebApi.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing to-do lists.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TodoListController : Controller
    {
        private readonly ITodoListDatabaseService repository;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListController"/> class.
        /// </summary>
        /// <param name="todoListDatabaseService">The service for interacting with to-do list data.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        public TodoListController(ITodoListDatabaseService todoListDatabaseService, ILogger<TodoListController> logger)
        {
            this.repository = todoListDatabaseService;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all to-do lists based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter parameters for retrieving to-do lists.</param>
        /// <returns>A list of to-do lists matching the filter criteria.</returns>
        [HttpGet]
        public async Task<ActionResult<TodoListDto>> GetAllLists([FromQuery] TodoListFilter filter)
        {
            this.logger.LogTrace(nameof(this.GetAllLists));

            var list = await this.repository.GetAllAsync(filter);
            if (list.Any())
            {
                return this.Ok(list);
            }

            this.logger.LogWarning("No lists found");
            return this.NotFound("No lists found");
        }

        /// <summary>
        /// Retrieves a specific to-do list by its ID.
        /// </summary>
        /// <param name="id">The ID of the to-do list.</param>
        /// <returns>The to-do list with the specified ID.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskDto>> GetTodoList(int id)
        {
            this.logger.LogTrace(nameof(this.GetTodoList));

            var list = await this.repository.GetByIdAsync(id);
            if (list != null)
            {
                return this.Ok(list);
            }

            this.logger.LogWarning("Request body cannot be empty.");
            return this.NotFound($"To-do list with {id} not found");
        }

        /// <summary>
        /// Updates an existing to-do list with the specified ID.
        /// </summary>
        /// <param name="model">The to-do list data to update.</param>
        /// <param name="id">The ID of the to-do list to update.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateList([FromBody] TodoListUpdateDto model, int id)
        {
            this.logger.LogTrace(nameof(this.UpdateList));

            if (model == null)
            {
                this.logger.LogWarning("Request body cannot be empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.repository.UpdateAsync(model, id);
            if (result)
            {
                return this.Ok();
            }

            this.logger.LogWarning($"TodoList with ID {id} not found.");
            return this.NotFound($"TodoList with ID {id} not found.");
        }

        /// <summary>
        /// Adds a new to-do list.
        /// </summary>
        /// <param name="model">The to-do list data to add.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] TodoListDto model)
        {
            this.logger.LogTrace(nameof(this.AddList));

            if (model == null)
            {
                this.logger.LogWarning("Request body cannot be empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            if (string.IsNullOrEmpty(model.OwnerId))
            {
                this.logger.LogWarning("User id cannot be empty");
                return this.BadRequest("User id cannot be empty");
            }

            await this.repository.CreateAsync(model);

            return this.Ok();
        }

        /// <summary>
        /// Deletes a to-do list with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the to-do list to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteList([FromRoute] int id)
        {
            this.logger.LogTrace(nameof(this.DeleteList));

            bool result = await this.repository.DeleteByIdAsync(id);
            if (result)
            {
                return this.NoContent();
            }

            this.logger.LogWarning($"TodoList with ID {id} not found.");
            return this.NotFound($"TodoList with ID {id} not found.");
        }
    }
}
