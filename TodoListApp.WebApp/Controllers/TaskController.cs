using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models.Enums;
using TodoListApp.WebApp.Models;
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
            return this.View(list);
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
    }
}
