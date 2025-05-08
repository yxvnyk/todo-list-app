using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.PagingDTO;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskDatabaseService taskService;
        private readonly ITodoListDatabaseService todoListService;
        private readonly ILogger logger;

        public TaskController(ITaskDatabaseService taskDatabaseService, ITodoListDatabaseService todoListService, ILogger<TaskController> logger)
        {
            this.taskService = taskDatabaseService;
            this.logger = logger;
            this.todoListService = todoListService;
        }

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
