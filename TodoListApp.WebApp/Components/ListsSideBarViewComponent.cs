using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using TodoListApp.WebApp.Services;

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
        var list = await this.apiService.GetAllTodoListsAsync();
        return this.View(list);
    }
}
