using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models.Logging;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers
{
    /// <summary>
    /// Controller for managing comment-related actions like create, update, and delete.
    /// </summary>
    public class CommentController : Controller
    {
        private readonly ICommentWebApiService apiService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentController"/> class.
        /// </summary>
        /// <param name="apiService">The service used to interact with the API for comment-related operations.</param>
        /// <param name="logger">Logger instance for logging actions and errors.</param>
        public CommentController(ICommentWebApiService apiService, ILogger<CommentController> logger)
        {
            this.apiService = apiService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new comment using the provided <see cref="CommentDto"/> and returns the user to the specified return URL.
        /// </summary>
        /// <param name="commentDto">The comment data transfer object containing the comment details.</param>
        /// <param name="returnUrl">The URL to which the user should be redirected after the operation.</param>
        /// <returns>Redirects to the provided return URL after the comment is created.</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(CommentDto commentDto, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Create));

            if (this.ModelState.IsValid)
            {
                this.logger.LogWarning("Invalid ModelState");
                _ = await this.apiService.AddAsync(commentDto);
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
        /// Updates an existing comment using the provided <see cref="CommentUpdateDto"/> and comment ID, and redirects to the specified return URL.
        /// </summary>
        /// <param name="commentDto">The comment update data transfer object containing the new comment details.</param>
        /// <param name="id">The ID of the comment to be updated.</param>
        /// <param name="returnUrl">The URL to which the user should be redirected after the operation.</param>
        /// <returns>Redirects to the provided return URL after the comment is updated.</returns>
        [HttpPost]
        [Route("edit/{id:int}")]
        public async Task<IActionResult> Update(CommentUpdateDto commentDto, int id, Uri returnUrl)
        {
            this.logger.LogTrace(nameof(this.Update));

            if (this.ModelState.IsValid)
            {
                this.logger.LogWarning("Invalid ModelState");
                _ = await this.apiService.UpdateAsync(commentDto, id);
            }

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl.ToString());
            }

            return this.Redirect("~/Home/Index");
        }

        /// <summary>
        /// Deletes a comment with the specified ID and redirects to the provided return URL.
        /// </summary>
        /// <param name="id">The ID of the comment to be deleted.</param>
        /// <param name="returnUrl">The URL to which the user should be redirected after the operation.</param>
        /// <returns>Redirects to the provided return URL after the comment is deleted.</returns>
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
