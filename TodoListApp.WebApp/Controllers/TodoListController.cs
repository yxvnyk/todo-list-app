using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Services;

namespace TodoListApp.WebApp.Controllers
{
    [Controller]
    [Route("TodoLists")]
    public class TodoListController: Controller
    {
        private readonly ITodoListWebApiService apiService;

        public TodoListController(ITodoListWebApiService todoListWebApi)
        {
            this.apiService = todoListWebApi;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllLists()
        {
            var list = await this.apiService.GetAllTodoListsAsync();
            if (list == null)
            {
                return this.NotFound();
            }

            this.Ok(list);
            return this.View(list);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> CreateTodos([FromBody] TodoListModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var list = await this.apiService.AddTodoListAsync(model);
            if (list == null)
            {
                return this.NotFound();
            }

            return this.Ok(list);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTodos(int id)
        {
            var list = await this.apiService.DeleteTodoListAsync(id);
            if (list == null)
            {
                return this.NotFound();
            }

            return this.Ok(list);
        }

        [HttpPut("Put")]
        public async Task<IActionResult> PutTodos([FromBody] TodoListModel model)
        {
            var list = await this.apiService.UpdateTodoListAsync(model);
            if (list == null)
            {
                return this.NotFound();
            }

            return this.Ok(list);
        }
    }
}
