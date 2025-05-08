using System.Security.Claims;
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
        private readonly ILogger logger;

        public TodoListController(ITodoListWebApiService todoListWebApi, ILogger<TodoListController> logger)
        {
            this.apiService = todoListWebApi;
            this.logger = logger;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllLists(string ownerId = "")
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllLists));

            var list = await this.apiService.GetAllAsync(ownerId);
            if (list == null)
            {
                LoggerExtensions.LogWarning(this.logger, "List equal to null");

                return this.NotFound();
            }

            _ = this.Ok(list);
            return this.View(list);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> CreateTodoList([FromBody] TodoListDTO model)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.CreateTodoList));

            ArgumentNullException.ThrowIfNull(model);
            var list = await this.apiService.AddAsync(model);
            if (list == null)
            {
                LoggerExtensions.LogWarning(this.logger, "List equal to null");

                return this.NotFound();
            }

            return this.Ok(list);
        }

        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(int listId, string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Delete));

            _ = await this.apiService.DeleteAsync(listId);
            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create(string returnUrl = "/")
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Create));

            TodoListDTO task = new TodoListDTO()
            {
                OwnerId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
            };
            return this.View((task, returnUrl));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(TodoListDTO list, string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Create));

            if (this.ModelState.IsValid)
            {
                LoggerExtensions.LogWarning(this.logger, "Invalid ModelState");

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
            LoggerExtensions.LogTrace(this.logger, nameof(this.Edit));

            var task = await this.apiService.GetByIdAsync(listId);
            return this.View((task, returnUrl));
        }

        [HttpPost]
        [Route("edit/{listId:int}")]
        public async Task<IActionResult> Update(TodoListUpdateDTO List, int listId, string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Update));

            if (this.ModelState.IsValid)
            {
                LoggerExtensions.LogWarning(this.logger, "Invalid ModelState");

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
