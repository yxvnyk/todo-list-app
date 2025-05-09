using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models.Logging;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers
{
    /// <summary>
    /// Controller for managing to-do lists through the Web API.
    /// </summary>
    [Route("TodoLists")]
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ITodoListWebApiService apiService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TodoListController"/> class.
        /// </summary>
        /// <param name="todoListWebApi">Service for to-do list operations.</param>
        /// <param name="logger">Logger instance.</param>
        public TodoListController(ITodoListWebApiService todoListWebApi, ILogger<TodoListController> logger)
        {
            this.apiService = todoListWebApi;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all to-do lists for a given owner.
        /// </summary>
        /// <param name="ownerId">Optional owner ID. If empty, all lists are fetched.</param>
        /// <returns>An <see cref="IActionResult"/> containing the to-do lists or a NotFound result.</returns>
        [HttpGet("Get")]
        public async Task<IActionResult> GetAllLists(string ownerId = "")
        {
            this.logger.LogTrace(nameof(this.GetAllLists));

            var list = await this.apiService.GetAllAsync(ownerId);
            if (list == null)
            {
                this.logger.LogWarning("List equal to null");

                return this.NotFound();
            }

            _ = this.Ok(list);
            return this.View(list);
        }

        /// <summary>
        /// Creates a new to-do list.
        /// </summary>
        /// <param name="model">The to-do list model.</param>
        /// <returns>An <see cref="IActionResult"/> containing the created list or a NotFound result.</returns>
        [HttpPost("Post")]
        public async Task<IActionResult> CreateTodoList([FromBody] TodoListDto model)
        {
            this.logger.LogTrace(nameof(this.CreateTodoList));

            var list = await this.apiService.AddAsync(model);
            if (list == null)
            {
                this.logger.LogWarning("List equal to null");

                return this.NotFound();
            }

            return this.Ok(list);
        }

        /// <summary>
        /// Deletes a to-do list by its ID.
        /// </summary>
        /// <param name="listId">The ID of the list to delete.</param>
        /// <param name="returnUrl">URL to redirect to after deletion.</param>
        /// <returns>A redirect to the specified return URL.</returns>
        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(int listId, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Delete));

            _ = await this.apiService.DeleteAsync(listId);
            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.Redirect("~/Home/Index");
        }

        /// <summary>
        /// Renders the to-do list creation form.
        /// </summary>
        /// <param name="returnUrl">URL to return to after creation.</param>
        /// <returns>A view with the to-do list model and return URL.</returns>
        [HttpGet]
        [Route("create")]
        public IActionResult Create(Uri returnUrl = default!)
        {
            this.logger.LogTrace(nameof(this.Create));

            TodoListDto task = new TodoListDto()
            {
                OwnerId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
            };
            return this.View((task, returnUrl));
        }

        /// <summary>
        /// Handles creation of a to-do list from form submission.
        /// </summary>
        /// <param name="list">The to-do list to create.</param>
        /// <param name="returnUrl">URL to return to after creation.</param>
        /// <returns>A view indicating completion or a redirect on failure.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(TodoListDto list, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Create));

            if (this.ModelState.IsValid)
            {
                this.logger.LogWarning("Invalid ModelState");

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

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.Redirect("~/Home/Index");
        }

        /// <summary>
        /// Renders the to-do list editing form.
        /// </summary>
        /// <param name="listId">The ID of the list to edit.</param>
        /// <param name="returnUrl">URL to return to after editing.</param>
        /// <returns>A view with the to-do list data and return URL.</returns>
        [HttpGet]
        [Route("edit/{listId:int}")]
        public async Task<IActionResult> Edit(int listId, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Edit));

            var task = await this.apiService.GetByIdAsync(listId);
            return this.View((task, returnUrl));
        }

        /// <summary>
        /// Updates an existing to-do list.
        /// </summary>
        /// <param name="list">The updated list data.</param>
        /// <param name="listId">The ID of the list to update.</param>
        /// <param name="returnUrl">URL to return to after update.</param>
        /// <returns>A view indicating completion or a redirect on failure.</returns>
        [HttpPost]
        [Route("edit/{listId:int}")]
        public async Task<IActionResult> Update(TodoListUpdateDto list, int listId, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Update));

            if (this.ModelState.IsValid)
            {
                this.logger.LogWarning("Invalid ModelState");

                _ = await this.apiService.UpdateAsync(list, listId);
                return this.View("CompleteEditor", new CompleteEditorViewModel()
                {
                    Title = "To-do list",
                    Method = "update",
                });
            }

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.Redirect("~/Home/Index");
        }
    }
}
