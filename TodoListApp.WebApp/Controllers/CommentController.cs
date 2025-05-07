using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

public class CommentController : Controller
{
    private readonly ICommentWebApiService apiService;

    public CommentController(ICommentWebApiService apiService)
    {
        this.apiService = apiService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(CommentDTO commentDto, string returnUrl)
    {
        ArgumentNullException.ThrowIfNull(commentDto);
        //commentDto.AuthorId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        if (this.ModelState.IsValid)
        {
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
        if (this.ModelState.IsValid)
        {
            _ = await this.apiService.UpdateAsync(comment, id);
        }

        return this.Redirect(returnUrl);
    }

    [HttpGet]
    [Route("delete")]
    public async Task<IActionResult> Delete(int id, string returnUrl)
    {
        _ = await this.apiService.DeleteAsync(id);
        return this.Redirect(returnUrl);
    }
}
