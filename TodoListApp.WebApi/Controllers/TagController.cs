using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.DataAccess.Filters;
using TodoListApp.WebApi.Data.Repository.Interfaces;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TagController : Controller
{
    private readonly ITagDatabaseService repository;
    private readonly ILogger logger;

    public TagController(ITagDatabaseService commentDatabaseService, ILogger<TagController> logger)
    {
        this.repository = commentDatabaseService;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<TagDTO>> GetAllTags([FromQuery] TagFilter filter)
    {
        var list = await this.repository.GetAllAsync(filter);
        if (list.Any())
        {
            list = list.DistinctBy(x => x.Name); // return without duplication
            return this.Ok(list);
        }

        LoggerExtensions.LogWarning(this.logger, "No tags found");
        return this.NotFound("No tags found");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTag([FromBody] TagUpdateDTO model, int id)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.UpdateTag));

        if (model == null)
        {
            LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
            return this.BadRequest("Request body cannot be empty.");
        }

        bool result = await this.repository.UpdateAsync(model, id);
        if (result)
        {
            return this.Ok();
        }

        LoggerExtensions.LogWarning(this.logger, $"Tag with ID {id} not found.");
        return this.NotFound($"Tag with ID {id} not found.");
    }

    [HttpPost]
    public async Task<IActionResult> AddTag([FromBody] TagDTO model)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.AddTag));

        if (model == null)
        {
            LoggerExtensions.LogWarning(this.logger, "Request body cannot be empty.");
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
    public async Task<IActionResult> DeleteTag([FromRoute] int id)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.DeleteTag));

        bool result = await this.repository.DeleteByIdAsync(id);
        if (result)
        {
            return this.NoContent();
        }

        LoggerExtensions.LogWarning(this.logger, $"Tag with ID {id} not found.");
        return this.NotFound($"Tag with ID {id} not found.");
    }
}
