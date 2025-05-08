using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

public class CommentController : Controller
{
    private readonly ICommentWebApiService apiService;
    private readonly ILogger logger;

    public CommentController(ICommentWebApiService apiService, ILogger<CommentController> logger)
    {
        this.apiService = apiService;
        this.logger = logger;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(CommentDTO commentDto, string returnUrl)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.Create));


        ArgumentNullException.ThrowIfNull(commentDto);
        if (this.ModelState.IsValid)
        {
            LoggerExtensions.LogWarning(this.logger, "Invalid ModelState");

            _ = await this.apiService.AddAsync(commentDto);
        }

        foreach (var error in this.ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine(error.ErrorMessage);
        }

        return this.Redirect(returnUrl);
    }

    [HttpPost]
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Update(CommentUpdateDTO comment, int id, string returnUrl)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.Update));

        if (this.ModelState.IsValid)
        {
            LoggerExtensions.LogWarning(this.logger, "Invalid ModelState");

            _ = await this.apiService.UpdateAsync(comment, id);
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
