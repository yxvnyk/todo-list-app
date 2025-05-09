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
using TodoListApp.WebApi.Models.Logging;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers
{
    /// <summary>
    /// Controller for handling task-related operations in the web application.
    /// </summary>
    [Route("task")]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskWebApiService apiService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskController"/> class.
        /// </summary>
        /// <param name="apiService">Service for interacting with task Web API.</param>
        /// <param name="logger">Logger instance.</param>
        public TaskController(ITaskWebApiService apiService, ILogger<TaskController> logger)
        {
            this.apiService = apiService;
            this.logger = logger;
        }

        /// <summary>
        /// Displays the search view for tasks.
        /// </summary>
        /// <returns>The search view.</returns>
        [HttpGet("Search")]
        public IActionResult Search()
        {
            this.logger.LogTrace(nameof(this.Search));
            return this.View();
        }

        /// <summary>
        /// Searches for tasks based on the specified query and criteria.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <param name="searchBy">The field to search by.</param>
        /// <param name="page">Page number for pagination.</param>
        /// <returns>The search results view.</returns>
        [HttpGet]
        public async Task<IActionResult> SearchBy(string query, string searchBy, int page = 1)
        {
            this.logger.LogTrace(nameof(this.SearchBy));

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

        /// <summary>
        /// Retrieves all tasks belonging to a specific list.
        /// </summary>
        /// <param name="listId">ID of the list.</param>
        /// <param name="page">Page number for pagination.</param>
        /// <returns>The task list view.</returns>
        [HttpGet("Get")]
        public async Task<IActionResult> GetAllTasksByListId(int listId, int page = 1)
        {
            this.logger.LogTrace(nameof(this.GetAllTasksByListId));

            TaskFilter filter = new TaskFilter()
            {
                TodoListId = listId,
                PageNumber = page,
            };
            var list = await this.apiService.GetAllByFilterAsync(filter);
            return this.View((list, listId));
        }

        /// <summary>
        /// Retrieves all tasks filtered by a tag.
        /// </summary>
        /// <param name="tag">The tag to filter by.</param>
        /// <param name="page">Page number for pagination.</param>
        /// <returns>The view with filtered tasks.</returns>
        [HttpGet("GetByTag")]
        public async Task<IActionResult> GetAllTasksByTag(string tag, int page = 1)
        {
            this.logger.LogTrace(nameof(this.GetAllTasksByTag));

            TaskFilter filter = new TaskFilter()
            {
                TagName = tag,
                PageNumber = page,
            };
            var list = await this.apiService.GetAllByFilterAsync(filter);
            return this.View((list, tag));
        }

        /// <summary>
        /// Retrieves all tasks assigned to the current user.
        /// </summary>
        /// <returns>The view with tasks assigned to the user.</returns>
        [HttpGet("GetByAssignee")]
        public async Task<IActionResult> GetAllTasksByAssigneeId()
        {
            this.logger.LogTrace(nameof(this.GetAllTasksByAssigneeId));

            var assigneeId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            TaskFilter filter = new TaskFilter()
            {
                AssigneeId = assigneeId,
                Overdue = Overdue.Active,
            };
            var list = await this.apiService.GetAllByFilterAsync(filter);
            return this.View((list, assigneeId));
        }

        /// <summary>
        /// Stores the provided filter in the session and redirects to filtered results.
        /// </summary>
        /// <param name="filter">Task filter to apply.</param>
        /// <param name="returnUrl">URL to return to after applying filter.</param>
        /// <returns>Redirection to filtered results view.</returns>
        [HttpPost("GetByFilter")]
        public IActionResult GetAllTasksByFilter(TaskFilter filter, Uri returnUrl = default!)
        {
            this.logger.LogTrace(nameof(this.GetAllTasksByFilter));

            this.HttpContext.Session.SetString("filter", JsonSerializer.Serialize(filter));
            return this.RedirectToAction("FilteredResults", new { returnUrl });
        }

        /// <summary>
        /// Displays results based on a previously stored filter.
        /// </summary>
        /// <param name="returnUrl">URL to return to if filter is missing.</param>
        /// <returns>The filtered results view.</returns>
        [HttpGet("FilteredResults")]
        public async Task<IActionResult> FilteredResults(Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.FilteredResults));

            var filterJson = this.HttpContext.Session.GetString("filter");
            if (!string.IsNullOrEmpty(filterJson))
            {
                var filter = JsonSerializer.Deserialize<TaskFilter>(filterJson);
                var list = await this.apiService.GetAllByFilterAsync(filter!);
                return this.View("FilteredResults", (list, filter));
            }

            this.logger.LogWarning("Json is null or empty");
            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.Redirect("~/Home/Index");
        }

        /// <summary>
        /// Marks a task as complete with a given status.
        /// </summary>
        /// <param name="id">Task ID.</param>
        /// <param name="status">New status for the task.</param>
        /// <param name="returnUrl">URL to return to after update.</param>
        /// <returns>Redirection to returnUrl.</returns>
        [HttpPost("complete")]
        public IActionResult TaskComplete(int id, Status status, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.TaskComplete));

            TaskUpdateDto model = new TaskUpdateDto()
            {
                Status = status,
            };
            _ = this.apiService.UpdateAsync(model, id);
            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.Redirect("~/Home/Index");
        }

        /// <summary>
        /// Displays the edit form for a task.
        /// </summary>
        /// <param name="taskId">Task ID to edit.</param>
        /// <param name="returnUrl">URL to return to after editing.</param>
        /// <returns>The edit view.</returns>
        [HttpGet]
        [Route("edit/{taskId:int}")]
        public async Task<IActionResult> Edit(int taskId, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Edit));

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var ownerId = await this.apiService.GetTaskOwnerId(taskId);
            if (userId != ownerId)
            {
                this.logger.LogWarning("User id don't equal owner id");
                return this.View("NoPermission", "edit this task");
            }

            var task = await this.apiService.GetByIdAsync(taskId);
            return this.View((task, returnUrl));
        }

        /// <summary>
        /// Updates the task with new values.
        /// </summary>
        /// <param name="task">Updated task model.</param>
        /// <param name="id">Task ID.</param>
        /// <param name="returnUrl">URL to return to.</param>
        /// <returns>The result view or the edit form with validation errors.</returns>
        [HttpPost]
        [Route("edit/{id:int}")]
        public async Task<IActionResult> Update(TaskUpdateDto task, int id, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Update));

            if (task == null)
            {
                this.logger.LogWarning("Task is empty");
                if (returnUrl != null)
                {
                    return this.Redirect(returnUrl.ToString());
                }

                return this.Redirect("~/Home/Index");
            }

            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.UpdateAsync(task, id);
                return this.View("CompleteEditor", new CompleteEditorViewModel()
                {
                    Title = "Task",
                    Method = "create",
                });
            }

            this.logger.LogWarning("Invalid ModelState");

            foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            var dto = new TaskDto
            {
                Id = id,
                Title = task.Title ?? string.Empty,
                Description = task.Description,
                DateCreated = task.DateCreated ?? DateTime.UtcNow,
                DueDate = task.DueDate,
                Status = task.Status ?? Status.NotStarted,
                AssigneeId = task.AssigneeId ?? string.Empty,
            };
            return this.View("Edit", (dto, returnUrl));
        }

        /// <summary>
        /// Deletes a task if the current user is the owner.
        /// </summary>
        /// <param name="taskId">Task ID to delete.</param>
        /// <param name="returnUrl">URL to return to after deletion.</param>
        /// <returns>Redirection to returnUrl or permission error view.</returns>
        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(int taskId, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Delete));

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var ownerId = await this.apiService.GetTaskOwnerId(taskId);
            if (userId != ownerId)
            {
                this.logger.LogWarning("User id don't equal owner id");

                return this.View("NoPermission", "to delete this task");
            }

            _ = await this.apiService.DeleteAsync(taskId);
            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.Redirect("~/Home/Index");
        }

        /// <summary>
        /// Displays the create task form.
        /// </summary>
        /// <param name="listId">The ID of the list the task will belong to.</param>
        /// <param name="returnUrl">URL to return to after creation.</param>
        /// <returns>The create task view.</returns>
        [HttpGet]
        [Route("create/{listId:int}")]
        public IActionResult Create(int listId, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Create));

            TaskDto task = new TaskDto()
            {
                TodoListId = listId,
                AssigneeId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
            };
            return this.View((task, returnUrl));
        }

        /// <summary>
        /// Submits a new task to be created.
        /// </summary>
        /// <param name="task">Task data to create.</param>
        /// <param name="returnUrl">URL to return to after creation.</param>
        /// <returns>The result view or the create form with validation errors.</returns>
        [HttpPost("create/{returnUrl}")]
        public async Task<IActionResult> Create(TaskDto task, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Create));

            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.AddAsync(task);
                return this.View("CompleteEditor", new CompleteEditorViewModel()
                {
                    Title = "Task",
                    Method = "create",
                });
            }

            this.logger.LogWarning("Invalid ModelState");

            foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return this.View("Create", (task, returnUrl));
        }
    }
}
