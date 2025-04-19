using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
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
        var list = await this.apiService.GetAllAsync(0);
        return this.View(list);
    }
}
