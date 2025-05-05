using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.DTO.UpdateDTO;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

[Route("tag")]
public class TagController : Controller
{
    private readonly ITagWebApiService apiService;

    public TagController(ITagWebApiService apiService)
    {
        this.apiService = apiService;
    }

    [HttpGet]
    [Route("TagMenu")]
    public async Task<IActionResult> TagMenu(string returnUrl, int id = 0)
    {
        var list = await this.apiService.GetAllAsync(id);
        if (list == null)
        {
            return this.Redirect(returnUrl);
        }

        _ = this.Ok(list);
        return this.View((list, returnUrl));
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(TagDTO tag, string returnUrl)
    {
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
        if (this.ModelState.IsValid)
        {
            _ = await this.apiService.UpdateAsync(tag, id);
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
