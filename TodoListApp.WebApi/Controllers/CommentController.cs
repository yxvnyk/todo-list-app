using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApi.Models.Logging;

namespace TodoListApp.WebApi.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing comments related to tasks.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentDatabaseService commentRepository;
        private readonly ITaskDatabaseService taskRepository;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentController"/> class.
        /// </summary>
        /// <param name="commentDatabaseService">The service for interacting with comment data.</param>
        /// <param name="taskRepository">The service for interacting with task data.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        public CommentController(ICommentDatabaseService commentDatabaseService, ITaskDatabaseService taskRepository, ILogger<CommentController> logger)
        {
            this.commentRepository = commentDatabaseService;
            this.taskRepository = taskRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all comments based on the specified filter.
        /// </summary>
        /// <param name="filter">The filter parameters for retrieving comments.</param>
        /// <returns>A list of comments matching the filter criteria.</returns>
        [HttpGet]
        public async Task<ActionResult<TagDto>> GetAllComments([FromQuery] CommentFilter filter)
        {
            this.logger.LogTrace(nameof(this.GetAllComments));

            var list = await this.commentRepository.GetAllAsync(filter);
            if (list.Any())
            {
                return this.Ok(list);
            }

            this.logger.LogWarning("No comments found");
            return this.NotFound("No comments found");
        }

        /// <summary>
        /// Updates an existing comment with the specified ID.
        /// </summary>
        /// <param name="model">The comment data to update.</param>
        /// <param name="id">The ID of the comment to update.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateDto model, int id)
        {
            this.logger.LogTrace(nameof(this.UpdateComment));

            if (model == null)
            {
                this.logger.LogWarning("Request body is empty");
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.commentRepository.UpdateAsync(model, id);
            if (result)
            {
                return this.Ok();
            }

            this.logger.LogWarning("Invalid comment id or invalid data");
            return this.NotFound($"Comment with ID {id} not found or invalid data");
        }

        /// <summary>
        /// Adds a new comment to a task.
        /// </summary>
        /// <param name="model">The comment data to add.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentDto model)
        {
            this.logger.LogTrace(nameof(this.AddComment));

            if (model == null)
            {
                this.logger.LogWarning("Request body is empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            var taskExist = await this.taskRepository.TaskExist(model.TaskId);
            if (!taskExist)
            {
                this.logger.LogWarning("Task with the given ID does not exist.");
                return this.BadRequest("Task with the given ID does not exist.");
            }

            await this.commentRepository.CreateAsync(model);

            return this.Ok();
        }

        /// <summary>
        /// Deletes a comment with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            this.logger.LogTrace($"Received request to {nameof(this.GetAllComments)}");

            bool result = await this.commentRepository.DeleteByIdAsync(id);
            if (result)
            {
                return this.NoContent();
            }

            this.logger.LogWarning("Comment with this id doesn't exist");
            return this.NotFound($"Comment with ID {id} not found.");
        }
    }
}
