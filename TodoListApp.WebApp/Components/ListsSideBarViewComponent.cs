using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Components;

public class ListsSideBarViewComponent : ViewComponent
{
    private readonly ITodoListWebApiService apiService;

    public ListsSideBarViewComponent(ITodoListWebApiService service)
    {
        this.apiService = service;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = this.User as ClaimsPrincipal;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (user == null)
        {
            return this.View(new List<TodoListDTO>());
        }

        var list = await this.apiService.GetAllAsync(userId);
        return this.View(list);
    }
}
