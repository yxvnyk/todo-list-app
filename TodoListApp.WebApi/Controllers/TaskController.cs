using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            var list = await this.repository.GetAll();
            if (list != null)
            {
                return this.Ok(list);
            }

            return this.NotFound("No task found");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTask(int id)
        {
            var list = await this.repository.GetById(id);
            if (list != null)
            {
                return this.Ok(list);
            }

            return this.NotFound("Task with {id} not found");
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

            await this.repository.Create(model);

            return this.Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskModel model, int id)
        {
            if (model == null)
            {
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.repository.Update(model, id);
            if (result)
            {
                return this.Ok();
            }

            return this.NotFound($"Task with ID {model.Id} not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            bool result = await this.repository.DeleteById(id);
            if (result)
            {
                return this.NoContent();
            }

            return this.NotFound($"Task with ID {id} not found.");
        }
    }
}
