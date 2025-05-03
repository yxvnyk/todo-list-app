using Microsoft.AspNetCore.Mvc;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoListController : Controller
    {
        private readonly ITodoListDatabaseService repository;
        private readonly ILogger logger;

        public TodoListController(ITodoListDatabaseService todoListDatabaseService, ILogger<TodoListController> logger)
        {
            this.repository = todoListDatabaseService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<TodoListDTO>> GetAllLists([FromQuery] TodoListFilter filter)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllLists));

            var list = await this.repository.GetAllAsync(filter);
            if (list.Any())
            {
                return this.Ok(list);
            }

            LoggerExtensions.LogWarning(this.logger, "No lists found");
            return this.NotFound("No lists found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskDTO>> GetTodoList(int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetTodoList));

            var list = await this.repository.GetByIdAsync(id);
            if (list != null)
            {
                return this.Ok(list);
            }

            LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
            return this.NotFound($"To-do list with {id} not found");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateList([FromBody] TodoListUpdateDTO model, int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.UpdateList));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.repository.UpdateAsync(model, id);
            if (result)
            {
                return this.Ok();
            }

            LoggerExtensions.LogWarning(this.logger, "TodoList with ID {model.Id} not found.");
            return this.NotFound($"TodoList with ID {id} not found.");
        }

        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] TodoListDTO model)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.AddList));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            // TODO : role-based verification
            if (string.IsNullOrEmpty(model.UserId))
            {
                LoggerExtensions.LogWarning(this.logger, "User id cannot be empty");
                return this.BadRequest("User id cannot be empty");
            }

            await this.repository.CreateAsync(model);

            return this.Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteList([FromRoute] int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.DeleteList));

            bool result = await this.repository.DeleteByIdAsync(id);
            if (result)
            {
                return this.NoContent();
            }

            LoggerExtensions.LogWarning(this.logger, "TodoList with ID {id} not found.");
            return this.NotFound($"TodoList with ID {id} not found.");
        }
    }
}
