using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models.Enums;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers
{
    [Route("task")]
    public class TaskController : Controller
    {
        private readonly ITaskWebApiService apiService;

        public TaskController(ITaskWebApiService apiService)
        {
            this.apiService = apiService;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllTasksByListId(int id)
        {
            var list = await this.apiService.GetAllByListAsync(id);
            return this.View((list, id));
        }

        [HttpPost("complete")]
        public IActionResult TaskComplete(int id, Status status, string returnUrl)
        {
            TaskUpdateDTO model = new TaskUpdateDTO()
            {
                Status = status,
            };
            _ = this.apiService.UpdateAsync(model, id);
            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, string returnUrl)
        {
            var task = await this.apiService.GetByIdAsync(id);
            return this.View((task, returnUrl));
        }

        [HttpPost]
        [Route("edit/{id:int}")]
        public async Task<IActionResult> Update(TaskUpdateDTO Task, int id, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.UpdateAsync(Task, id);
                return this.View("CompleteEditor", "update");
            }

            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            _ = await this.apiService.DeleteAsync(id);
            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("create/{id:int}")]
        public async Task<IActionResult> Create(int id, string returnUrl)
        {
            TaskDTO task = new TaskDTO()
            {
                TodoListId = id,
            };
            return this.View((task, returnUrl));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(TaskDTO task, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.AddAsync(task);
                return this.View("CompleteEditor", "create");
            }

            foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return this.Redirect(returnUrl);
        }
    }
}
