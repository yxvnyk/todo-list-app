using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentDatabaseService commentRepository;
        private readonly ITaskDatabaseService taskRepository;
        private readonly ILogger logger;

        public CommentController(ICommentDatabaseService commentDatabaseService, ITaskDatabaseService taskRepository, ILogger<CommentController> logger)
        {
            this.commentRepository = commentDatabaseService;
            this.taskRepository = taskRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<TagDTO>> GetAllComments([FromQuery] CommentFilter filter)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.GetAllComments));

            var list = await this.commentRepository.GetAllAsync(filter);
            if (list.Any())
            {
                return this.Ok(list);
            }

            LoggerExtensions.LogWarning(this.logger, "No comments found");
            return this.NotFound("No comments found");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateDTO model, int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.UpdateComment));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body is empty");
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.commentRepository.UpdateAsync(model, id);
            if (result)
            {
                return this.Ok();
            }

            LoggerExtensions.LogWarning(this.logger, "Invalid comment id or invalid data");
            return this.NotFound($"Comment with ID {id} not found or invalid data");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentDTO model)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.AddComment));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body is empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            var taskExist = await this.taskRepository.TaskExist(model.TaskId);
            if (!taskExist)
            {
                LoggerExtensions.LogWarning(this.logger, "Task with the given ID does not exist.");
                return this.BadRequest("Task with the given ID does not exist.");
            }

            await this.commentRepository.CreateAsync(model);

            return this.Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.DeleteComment));

            bool result = await this.commentRepository.DeleteByIdAsync(id);
            if (result)
            {
                return this.NoContent();
            }

            LoggerExtensions.LogWarning(this.logger, "Comment with this id doesn't exist");
            return this.NotFound($"Comment with ID {id} not found.");
        }
    }
}
