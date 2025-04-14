using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Helpers;
using TodoListApp.WebApi.Helpers.Filters;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentDatabaseService repository;

        private readonly ILogger logger;

        public CommentController(ICommentDatabaseService commentDatabaseService, ILogger<CommentController> logger)
        {
            this.repository = commentDatabaseService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<TagModel>> GetAllComments([FromQuery] CommentFilter filter)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllComments));

            var list = await this.repository.GetAllAsync(filter);
            if (list.Any())
            {
                return this.Ok(list);
            }

            LoggerExtensions.LogWarning(this.logger, "No comments found");
            return this.NotFound("No comments found");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentModel model, int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.UpdateComment));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body is empty");
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.repository.UpdateAsync(model, id);
            if (result)
            {
                return this.Ok();
            }

            LoggerExtensions.LogWarning(this.logger, "Invalid comment id or invalid data");
            return this.NotFound($"Comment with ID {model.Id} not found or invalid data");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentModel model)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.AddComment));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body is empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            var taskExist = await this.repository.TaskExist(model.TaskId);
            if (!taskExist)
            {
                LoggerExtensions.LogWarning(this.logger, "Task with the given ID does not exist.");
                return this.BadRequest("Task with the given ID does not exist.");
            }

            await this.repository.CreateAsync(model);

            return this.Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.DeleteComment));

            bool result = await this.repository.DeleteByIdAsync(id);
            if (result)
            {
                return this.NoContent();
            }

            LoggerExtensions.LogWarning(this.logger, "Comment with this id doesn't exist");
            return this.NotFound($"Comment with ID {id} not found.");
        }
    }
}
