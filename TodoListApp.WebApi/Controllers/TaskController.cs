using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing tasks in a to-do list.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskDatabaseService taskService;
        private readonly ITodoListDatabaseService todoListService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController"/> class.
        /// </summary>
        /// <param name="taskDatabaseService">The service for interacting with task data.</param>
        /// <param name="todoListService">The service for interacting with to-do list data.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        public TaskController(ITaskDatabaseService taskDatabaseService, ITodoListDatabaseService todoListService, ILogger<TaskController> logger)
        {
            this.taskService = taskDatabaseService;
            this.logger = logger;
            this.todoListService = todoListService;
        }

        /// <summary>
        /// Retrieves the owner ID of the task specified by its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task.</param>
        /// <returns>The owner ID of the specified task.</returns>
        [HttpGet("GetOwnerId")]
        public ActionResult<string> GetTaskOwnerId(int taskId)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetTaskOwnerId));
            var id = this.taskService.GetTaskOwnerId(taskId);
            if (id != null)
            {
                return this.Ok(id);
            }

            LoggerExtensions.LogWarning(this.logger, "OwnerId not found");
            return this.NotFound("OwnerId not found");
        }

        /// <summary>
        /// Retrieves all tasks based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter parameters for retrieving tasks.</param>
        /// <returns>A list of tasks matching the filter criteria.</returns>
        [HttpGet]
        public async Task<ActionResult<TaskDTO>> GetAllTasks([FromQuery] TaskFilter filter)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllTasks));

            var taskPaging = await this.taskService.GetAllAsync(filter);
            if (taskPaging.Items != null)
            {
                return this.Ok(taskPaging);
            }

            LoggerExtensions.LogWarning(this.logger, "No tasks found");
            return this.NotFound("No tasks found");
        }

        /// <summary>
        /// Retrieves tasks based on the provided filter using a POST request.
        /// </summary>
        /// <param name="filter">The filter parameters for retrieving tasks.</param>
        /// <returns>A list of tasks matching the filter criteria.</returns>
        [HttpPost("search")]
        public async Task<ActionResult<TaskPaging>> GetAllTasksByPost([FromBody] TaskFilter filter)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllTasks));

            var taskPaging = await this.taskService.GetAllAsync(filter);
            if (taskPaging.Items != null)
            {
                return this.Ok(taskPaging);
            }

            LoggerExtensions.LogWarning(this.logger, "No tasks found");
            return this.NotFound("No tasks found");
        }

        /// <summary>
        /// Retrieves a task by its ID.
        /// </summary>
        /// <param name="id">The ID of the task.</param>
        /// <returns>The task with the specified ID.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskDTO>> GetTask(int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetTask));

            var list = await this.taskService.GetByIdAsync(id);
            if (list != null)
            {
                return this.Ok(list);
            }

            LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
            return this.NotFound($"Task with {id} not found");
        }

        /// <summary>
        /// Adds a new task to the specified to-do list.
        /// </summary>
        /// <param name="model">The task data to add.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskDTO model)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.AddTask));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.NotFound(this.ModelState);
            }

            var taskExist = await this.todoListService.TodoListExist(model!.TodoListId);
            if (!taskExist)
            {
                LoggerExtensions.LogWarning(this.logger, "To-do list with the given ID does not exist.");
                return this.BadRequest("To-do list with the given ID does not exist.");
            }

            await this.taskService.CreateAsync(model);

            return this.Ok();
        }

        /// <summary>
        /// Updates an existing task with the specified ID.
        /// </summary>
        /// <param name="model">The task data to update.</param>
        /// <param name="id">The ID of the task to update.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskUpdateDTO model, int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.UpdateTask));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.taskService.UpdateAsync(model, id);
            if (result)
            {
                return this.Ok();
            }

            LoggerExtensions.LogWarning(this.logger, "Task with ID {model.Id} not found.");
            return this.NotFound($"Task with ID {id} not found.");
        }

        /// <summary>
        /// Deletes a task with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.DeleteTask));

            bool result = await this.taskService.DeleteByIdAsync(id);
            if (result)
            {
                return this.NoContent();
            }

            LoggerExtensions.LogWarning(this.logger, "Task with ID {id} not found");
            return this.NotFound($"Task with ID {id} not found.");
        }
    }
}
