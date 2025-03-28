using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api[controller]")]
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
            var list = await this.repository.GetAllAsync();
            if (list != null)
            {
                return this.Ok(list);
            }

            return this.NotFound("No lists found");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateList([FromBody] TodoListModel model, int id)
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

            return this.NotFound($"TodoList with ID {model.Id} not found.");
        }

        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] TodoListModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Request body cannot be empty.");
            }

            //TODO : role-based verification
            if (string.IsNullOrEmpty(model.UserId))
            {
                return this.BadRequest("User id cannot be empty");
            }

            await this.repository.CreateAsync(model);

            return this.Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteList([FromRoute] int id)
        {
            bool result = await this.repository.DeleteByIdAsync(id);
            if (result)
            {
                return this.NoContent();
            }

            return this.NotFound($"TodoList with ID {id} not found.");
        }
    }
}
