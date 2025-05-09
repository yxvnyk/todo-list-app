using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models.Logging;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers
{
    /// <summary>
    /// Controller for managing tags associated with tasks.
    /// </summary>
    [Route("tag")]
    public class TagController : Controller
    {
        private readonly ITagWebApiService apiService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagController"/> class.
        /// </summary>
        /// <param name="apiService">Service for interacting with the tag-related API.</param>
        /// <param name="logger">Logger for logging activities within the controller.</param>
        public TagController(ITagWebApiService apiService, ILogger<TagController> logger)
        {
            this.apiService = apiService;
            this.logger = logger;
        }

        /// <summary>
        /// Displays the tag management menu.
        /// Retrieves all tags associated with the current user and returns them to the view.
        /// </summary>
        /// <param name="returnUrl">The URL to return to after completing the action.</param>
        /// <returns>The view displaying the tag menu with the list of tags.</returns>
        [HttpGet]
        [Route("TagMenu")]
        public async Task<IActionResult> TagMenu(Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.TagMenu));

            var user = this.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user == null)
            {
                return this.View(new List<TodoListDto>());
            }

            var list = await this.apiService.GetAllAsync(userId!, userId!);

            _ = this.Ok(list);
            return this.View((list, returnUrl));
        }

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="tag">The tag information to be added.</param>
        /// <param name="returnUrl">The URL to return to after completing the action.</param>
        /// <returns>A redirection to the specified return URL after the tag is created.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(TagDto tag, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Create));

            if (this.ModelState.IsValid)
            {
                _ = await this.apiService.AddAsync(tag);
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
        /// Updates an existing tag by its ID.
        /// </summary>
        /// <param name="tag">The updated tag information.</param>
        /// <param name="id">The ID of the tag to be updated.</param>
        /// <param name="returnUrl">The URL to return to after completing the action.</param>
        /// <returns>A redirection to the specified return URL after the tag is updated.</returns>
        [HttpPost]
        [Route("edit/{id:int}")]
        public async Task<IActionResult> Update(TagUpdateDto tag, int id, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Update));

            if (this.ModelState.IsValid)
            {
                this.logger.LogTrace("Invalid ModelState");

                _ = await this.apiService.UpdateAsync(tag, id);
            }

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.Redirect("~/Home/Index");
        }

        /// <summary>
        /// Deletes a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag to be deleted.</param>
        /// <param name="returnUrl">The URL to return to after completing the action.</param>
        /// <returns>A redirection to the specified return URL after the tag is deleted.</returns>
        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Delete));

            _ = await this.apiService.DeleteAsync(id);
            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.Redirect("~/Home/Index");
        }
    }
}
