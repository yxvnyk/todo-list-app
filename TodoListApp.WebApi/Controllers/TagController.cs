using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/tag")]
public class TagController : Controller
{
    private readonly ITagDatabaseService repository;

    public TagController(ITagDatabaseService commentDatabaseService)
    {
        this.repository = commentDatabaseService;
    }

    [HttpGet]
    public async Task<ActionResult<TagModel>> GetAllTags()
    {
        var list = await this.repository.GetAll();
        if (list != null)
        {
            return this.Ok(list);
        }

        return this.NotFound("No tags found");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag([FromBody] TagModel model, int id)
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

        return this.NotFound($"Tag with ID {model.Id} not found.");
    }

    [HttpPost]
    public async Task<IActionResult> AddTag([FromBody] TagModel model)
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
    public async Task<IActionResult> DeleteTag([FromRoute] int id)
    {
        bool result = await this.repository.DeleteById(id);
        if (result)
        {
            return this.NoContent();
        }

        return this.NotFound($"Tag with ID {id} not found.");
    }
}
