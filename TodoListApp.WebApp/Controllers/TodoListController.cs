using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers
{
    [Route("TodoLists")]
    public class TodoListController: Controller
    {
        private readonly ITodoListWebApiService apiService;

        public TodoListController(ITodoListWebApiService todoListWebApi)
        {
            this.apiService = todoListWebApi;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllLists(int id = 0)
        {
            var list = await this.apiService.GetAllAsync(id);
            if (list == null)
            {
                return this.NotFound();
            }

            _ = this.Ok(list);
            return this.View(list);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> CreateTodoList([FromBody] TodoListDTO model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var list = await this.apiService.AddAsync(model);
            if (list == null)
            {
                return this.NotFound();
            }

            return this.Ok(list);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteTodoList(int id)
        {
            var list = await this.apiService.DeleteAsync(id);
            if (list == null)
            {
                return this.NotFound();
            }

            return this.Ok(list);
        }

        [HttpPut("Put")]
        public async Task<IActionResult> UpdateTodoList([FromBody] TodoListUpdateDTO model, int id)
        {
            var list = await this.apiService.UpdateAsync(model, id);
            if (list == null)
            {
                return this.NotFound();
            }

            return this.Ok(list);
        }
    }
}
