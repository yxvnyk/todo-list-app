using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskDatabaseService repository;

        public TaskController(ITaskDatabaseService taskDatabaseService)
        {
            this.repository = taskDatabaseService;
        }

        [HttpGet]
        public async Task<ActionResult<TaskModel>> GetAllTasks()
        {
            var list = await this.repository.GetAllAsync();
            if (list != null)
            {
                return this.Ok(list);
            }

            return this.NotFound("No task found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskModel>> GetTask(int id)
        {
            var list = await this.repository.GetByIdAsync(id);
            if (list != null)
            {
                return this.Ok(list);
            }

            return this.NotFound($"Task with {id} not found");
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Request body cannot be empty.");
            }

            var taskExist = await this.repository.TodoListExist(model.TodoListId);
            if (!taskExist)
            {
                return this.BadRequest("To-do list with the given ID does not exist.");
            }

            await this.repository.CreateAsync(model);

            return this.Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskModel model, int id)
        {
            if (model == null)
            {
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.repository.UpdateAsync(model, id);
            if (result)
            {
                return this.Ok();
            }

            return this.NotFound($"Task with ID {model.Id} not found.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            bool result = await this.repository.DeleteByIdAsync(id);
            if (result)
            {
                return this.NoContent();
            }

            return this.NotFound($"Task with ID {id} not found.");
        }
    }
}
