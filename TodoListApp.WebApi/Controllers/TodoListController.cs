using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Repository;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoListController : Controller
    {
        private readonly ITodoListDatabaseService repository;

        public TodoListController(ITodoListDatabaseService todoListDatabaseService)
        {
            this.repository = todoListDatabaseService;
        }

        [HttpGet]
        public async Task<ActionResult<TodoListModel>> GetAllLists()
        {
            var list = await this.repository.GetAllTodoList();
            if (list != null)
            {
                return this.Ok(list);
            }

            return this.NotFound("No lists found");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateList([FromBody] TodoListModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.repository.UpdateTodoList(model);
            if (result)
            {
                return this.Ok();
            }

            return this.NotFound("TodoList with ID { model.Id} not found.");
        }

        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] TodoListModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Request body cannot be empty.");
            }

            await this.repository.AddTodoList(model);

            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList([FromRoute] int id)
        {
            bool result = await this.repository.DeleteByIdTodoList(id);
            if (result)
            {
                return this.NoContent();
            }

            return this.NotFound("TodoList with ID {model.Id} not found.");
        }
    }
}
