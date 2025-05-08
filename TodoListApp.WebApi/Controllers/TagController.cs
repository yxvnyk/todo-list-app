using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing tags related to tasks.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private readonly ITagDatabaseService tagRepository;
        private readonly ITaskDatabaseService taskRepository;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagController"/> class.
        /// </summary>
        /// <param name="commentDatabaseService">The service for interacting with tag data.</param>
        /// <param name="taskRepository">The service for interacting with task data.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        public TagController(ITagDatabaseService commentDatabaseService, ITaskDatabaseService taskRepository, ILogger<TagController> logger)
        {
            this.tagRepository = commentDatabaseService;
            this.taskRepository = taskRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves all tags based on the specified filter, without duplicates.
        /// </summary>
        /// <param name="filter">The filter parameters for retrieving tags.</param>
        /// <returns>A list of tags matching the filter criteria.</returns>
        [HttpGet]
        public async Task<ActionResult<TagDTO>> GetAllTags([FromQuery] TagFilter filter)
        {
            var list = await this.tagRepository.GetAllAsync(filter);
            if (list.Any())
            {
                list = list.DistinctBy(x => x.Name); // return without duplication
                return this.Ok(list);
            }

            LoggerExtensions.LogWarning(this.logger, "No tags found");
            return this.NotFound("No tags found");
        }

        /// <summary>
        /// Updates an existing tag with the specified ID.
        /// </summary>
        /// <param name="model">The tag data to update.</param>
        /// <param name="id">The ID of the tag to update.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTag([FromBody] TagUpdateDTO model, int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.UpdateTag));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.tagRepository.UpdateAsync(model, id);
            if (result)
            {
                return this.Ok();
            }

            LoggerExtensions.LogWarning(this.logger, $"Tag with ID {id} not found.");
            return this.NotFound($"Tag with ID {id} not found.");
        }

        /// <summary>
        /// Adds a new tag to a task.
        /// </summary>
        /// <param name="model">The tag data to add.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddTag([FromBody] TagDTO model)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.AddTag));

            if (model == null)
            {
                LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
                return this.BadRequest("Request body cannot be empty.");
            }

            var taskExist = await this.taskRepository.TaskExist(model.TaskId);
            if (!taskExist)
            {
                LoggerExtensions.LogWarning(this.logger, "Task with the given ID does not exist.");
                return this.BadRequest("Task with the given ID does not exist.");
            }

            await this.tagRepository.CreateAsync(model);

            return this.Ok();
        }

        /// <summary>
        /// Deletes a tag with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTag([FromRoute] int id)
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.DeleteTag));

            bool result = await this.tagRepository.DeleteByIdAsync(id);
            if (result)
            {
                return this.NoContent();
            }

            LoggerExtensions.LogWarning(this.logger, $"Tag with ID {id} not found.");
            return this.NotFound($"Tag with ID {id} not found.");
        }
    }
}
