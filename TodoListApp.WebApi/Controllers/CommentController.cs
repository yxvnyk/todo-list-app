using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentDatabaseService repository;

        public CommentController(ICommentDatabaseService commentDatabaseService)
        {
            this.repository = commentDatabaseService;
        }

        [HttpGet]
        public async Task<ActionResult<TagModel>> GetAllComments()
        {
            var list = await this.repository.GetAll();
            if (list != null)
            {
                return this.Ok(list);
            }

            return this.NotFound("No comment found");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentModel model, int id)
        {
            if (model == null)
            {
                return this.BadRequest("Request body cannot be empty.");
            }

            bool result = await this.repository.Update(model, id);
            if (result)
            {
                return this.Ok();
            }

            return this.NotFound($"Comment with ID {model.Id} not found or invalid data");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Request body cannot be empty.");
            }

            var taskExist = await this.repository.TaskExist(model.TaskId);
            if (!taskExist)
            {
                return this.BadRequest("Task with the given ID does not exist.");
            }

            await this.repository.Create(model);

            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            bool result = await this.repository.DeleteById(id);
            if (result)
            {
                return this.NoContent();
            }

            return this.NotFound($"Comment with ID {id} not found.");
        }
    }
}
