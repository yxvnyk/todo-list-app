using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.DataAccess.Filters;
using TodoListApp.DataAccess.Filters.Enums;
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

        [HttpGet("Search")]
        public IActionResult Search()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchBy(string query, string searchBy, int page = 1)
        {
            TaskFilter filter = new TaskFilter()
            {
                PageNumber = page,
            };
            DateTime date;
            switch (searchBy)
            {
                case "Title": filter.TextInTitle = query; break;
                case "DueDate":
                    if (DateTime.TryParseExact(query, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        filter.DueDate = date;
                    }

                    break;
                case "CreationDate":
                    if (DateTime.TryParseExact(query, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        filter.CreationDate = date;
                    }

                    break;
                default:
                    break;
            }

            var list = await this.apiService.GetAllByFilterAsync(filter);
            return this.View("Search", new SearchViewModel()
            {
                Query = query,
                SearchBy = searchBy,
                TaskPagging = list,
            });
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAllTasksByListId(int listId, int page = 1)
        {
            TaskFilter filter = new TaskFilter()
            {
                TodoListId = listId,
                PageNumber = page,
            };
            var list = await this.apiService.GetAllByFilterAsync(filter);
            return this.View((list, listId));
        }

        [HttpGet("GetByTag")]
        public async Task<IActionResult> GetAllTasksByTag(string tag, int page = 1)
        {
            TaskFilter filter = new TaskFilter()
            {
                TagName = tag,
                PageNumber = page,
            };
            var list = await this.apiService.GetAllByFilterAsync(filter);
            return this.View((list, tag));
        }

        [HttpGet("GetByAssignee")]
        public async Task<IActionResult> GetAllTasksByAssigneeId(string assigneeId)
        {
            TaskFilter filter = new TaskFilter()
            {
                AssigneeId = assigneeId,
                Overdue = Overdue.Active,
            };
            var list = await this.apiService.GetAllByFilterAsync(filter);
            return this.View((list, assigneeId));
        }

        [HttpPost("GetByFilter")]
        public IActionResult GetAllTasksByFilter(TaskFilter filter, string returnUrl = "/")
        {
            this.HttpContext.Session.SetString("filter", JsonSerializer.Serialize(filter));
            return this.RedirectToAction("FilteredResults", new { returnUrl = returnUrl });
        }

        [HttpGet("FilteredResults")]
        public async Task<IActionResult> FilteredResults(string returnUrl)
        {
            var filterJson = this.HttpContext.Session.GetString("filter");
            if (!string.IsNullOrEmpty(filterJson))
            {
                var filter = JsonSerializer.Deserialize<TaskFilter>(filterJson);
                var list = await this.apiService.GetAllByFilterAsync(filter!);
                return this.View("FilteredResults", (list, filter));
            }

            return this.Redirect(returnUrl);
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
                return this.View("CompleteEditor", new CompleteEditorViewModel()
                {
                    Title = "Task",
                    Method = "create",
                });
            }

            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(int listId, string returnUrl)
        {
            _ = await this.apiService.DeleteAsync(listId);
            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("create/{listId:int}")]
        public IActionResult Create(int listId, string returnUrl)
        {
            TaskDTO task = new TaskDTO()
            {
                TodoListId = listId,
            };
            return this.View((task, returnUrl));
        }

        [HttpPost("create/{returnUrl}")]
        public async Task<IActionResult> Create(TaskDTO task, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.AddAsync(task);
                return this.View("CompleteEditor", new CompleteEditorViewModel()
                {
                    Title = "Task",
                    Method = "create",
                });
            }

            foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return this.Redirect(returnUrl);
        }
    }
}
