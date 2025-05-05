using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers
{
    [Route("TodoLists")]
    [Authorize]
    public class TodoListController: Controller
    {
        private readonly ITodoListWebApiService apiService;

        public TodoListController(ITodoListWebApiService todoListWebApi)
        {
            this.apiService = todoListWebApi;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllLists(int listId = 0)
        {
            var list = await this.apiService.GetAllAsync(listId);
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

        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(int listId, string returnUrl)
        {
            _ = await this.apiService.DeleteAsync(listId);
            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("create/{id:alpha}")]
        public async Task<IActionResult> Create(string id, string returnUrl)
        {
            TodoListDTO task = new TodoListDTO()
            {
                UserId = id,
            };
            return this.View((task, returnUrl));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(TodoListDTO list, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.AddAsync(list);
                return this.View("CompleteEditor", new CompleteEditorViewModel()
                {
                    Title = "To-do list",
                    Method = "create",
                });
            }

            foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("edit/{listId:int}")]
        public async Task<IActionResult> Edit(int listId, string returnUrl)
        {
            var task = await this.apiService.GetByIdAsync(listId);
            return this.View((task, returnUrl));
        }

        [HttpPost]
        [Route("edit/{listId:int}")]
        public async Task<IActionResult> Update(TodoListUpdateDTO List, int listId, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.UpdateAsync(List, listId);
                return this.View("CompleteEditor", new CompleteEditorViewModel()
                {
                    Title = "To-do list",
                    Method = "update",
                });
            }

            return this.Redirect(returnUrl);
        }
    }
}
