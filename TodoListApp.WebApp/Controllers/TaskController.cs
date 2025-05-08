using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskWebApiService apiService;
        private readonly ILogger logger;

        public TaskController(ITaskWebApiService apiService, ILogger<TaskController> logger)
        {
            this.apiService = apiService;
            this.logger = logger;
        }

        [HttpGet("Search")]
        public IActionResult Search()
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Search));

            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchBy(string query, string searchBy, int page = 1)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.SearchBy));

            var assigneeId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            TaskFilter filter = new TaskFilter()
            {
                PageNumber = page,
                AssigneeId = assigneeId,
                OwnerId = assigneeId,
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
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllTasksByListId));

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
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllTasksByTag));

            TaskFilter filter = new TaskFilter()
            {
                TagName = tag,
                PageNumber = page,
            };
            var list = await this.apiService.GetAllByFilterAsync(filter);
            return this.View((list, tag));
        }

        [HttpGet("GetByAssignee")]
        public async Task<IActionResult> GetAllTasksByAssigneeId()
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllTasksByAssigneeId));

            var assigneeId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
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
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllTasksByFilter));

            this.HttpContext.Session.SetString("filter", JsonSerializer.Serialize(filter));
            return this.RedirectToAction("FilteredResults", new { returnUrl = returnUrl });
        }

        [HttpGet("FilteredResults")]
        public async Task<IActionResult> FilteredResults(string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.FilteredResults));

            var filterJson = this.HttpContext.Session.GetString("filter");
            if (!string.IsNullOrEmpty(filterJson))
            {
                var filter = JsonSerializer.Deserialize<TaskFilter>(filterJson);
                var list = await this.apiService.GetAllByFilterAsync(filter!);
                return this.View("FilteredResults", (list, filter));
            }

            LoggerExtensions.LogWarning(this.logger, "Json is null or empty");
            return this.Redirect(returnUrl);
        }

        [HttpPost("complete")]
        public IActionResult TaskComplete(int id, Status status, string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.TaskComplete));

            TaskUpdateDTO model = new TaskUpdateDTO()
            {
                Status = status,
            };
            _ = this.apiService.UpdateAsync(model, id);
            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("edit/{taskId:int}")]
        public async Task<IActionResult> Edit(int taskId, string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Edit));

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var ownerId = await this.apiService.GetTaskOwnerId(taskId);
            if (userId != ownerId)
            {
                LoggerExtensions.LogWarning(this.logger, "User id don't equal owner id");
                return this.View("NoPermission", "edit this task");
            }

            var task = await this.apiService.GetByIdAsync(taskId);
            return this.View((task, returnUrl));
        }

        [HttpPost]
        [Route("edit/{id:int}")]
        public async Task<IActionResult> Update(TaskUpdateDTO Task, int id, string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Update));

            if (Task == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Task is empty");

                return this.Redirect(returnUrl);
            }

            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.UpdateAsync(Task, id);
                return this.View("CompleteEditor", new CompleteEditorViewModel()
                {
                    Title = "Task",
                    Method = "create",
                });
            }

            LoggerExtensions.LogWarning(this.logger, "Invalid ModelState");

            foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            var dto = new TaskDTO
            {
                Id = id,
                Title = Task.Title ?? string.Empty,
                Description = Task.Description,
                DateCreated = Task.DateCreated ?? DateTime.UtcNow,
                DueDate = Task.DueDate,
                Status = Task.Status ?? Status.NotStarted,
                AssigneeId = Task.AssigneeId ?? string.Empty,
            };
            return this.View("Edit", (dto, returnUrl));
        }

        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(int taskId, string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Delete));

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var ownerId = await this.apiService.GetTaskOwnerId(taskId);
            if (userId != ownerId)
            {
                LoggerExtensions.LogWarning(this.logger, "User id don't equal owner id");

                return this.View("NoPermission", "to delete this task");
            }

            _ = await this.apiService.DeleteAsync(taskId);
            return this.Redirect(returnUrl);
        }

        [HttpGet]
        [Route("create/{listId:int}")]
        public IActionResult Create(int listId, string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Create));

            TaskDTO task = new TaskDTO()
            {
                TodoListId = listId,
                AssigneeId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
            };
            return this.View((task, returnUrl));
        }

        [HttpPost("create/{returnUrl}")]
        public async Task<IActionResult> Create(TaskDTO task, string returnUrl)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Create));

            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.AddAsync(task);
                return this.View("CompleteEditor", new CompleteEditorViewModel()
                {
                    Title = "Task",
                    Method = "create",
                });
            }

            LoggerExtensions.LogWarning(this.logger, "Invalid ModelState");

            foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return this.View("Create", (task, returnUrl));
        }
    }
}
