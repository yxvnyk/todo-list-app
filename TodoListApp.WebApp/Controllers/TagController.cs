using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

[Route("tag")]
public class TagController : Controller
{
    private readonly ITagWebApiService apiService;
    private readonly ILogger logger;

    public TagController(ITagWebApiService apiService, ILogger<TagController> logger)
    {
        this.apiService = apiService;
        this.logger = logger;
    }

    [HttpGet]
    [Route("TagMenu")]
    public async Task<IActionResult> TagMenu(string returnUrl)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.TagMenu));

        var user = this.User;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (user == null)
        {
            return this.View(new List<TodoListDTO>());
        }

        var list = await this.apiService.GetAllAsync(userId!, userId!);

        _ = this.Ok(list);
        return this.View((list, returnUrl));
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(TagDTO tag, string returnUrl)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.Create));

        if (this.ModelState.IsValid)
        {
            _ = await this.apiService.AddAsync(tag);
        }

        foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine(error.ErrorMessage);
        }

        return this.Redirect(returnUrl);
    }

    [HttpPost]
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Update(TagUpdateDTO tag, int id, string returnUrl)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.Update));

        if (this.ModelState.IsValid)
        {
            LoggerExtensions.LogTrace(this.logger, "Invalid ModelState");

            _ = await this.apiService.UpdateAsync(tag, id);
        }

        return this.Redirect(returnUrl);
    }

    [HttpGet]
    [Route("delete")]
    public async Task<IActionResult> Delete(int id, string returnUrl)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.Delete));

        _ = await this.apiService.DeleteAsync(id);
        return this.Redirect(returnUrl);
    }
}
