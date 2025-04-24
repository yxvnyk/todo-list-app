using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Infrastructure;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers;

[Route("details")]
public class DetailsController : Controller
{
    private readonly TaskAggregatorService service;

    public DetailsController(TaskAggregatorService service)
    {
        this.service = service;
    }

    [Route("task")]
    public async Task<IActionResult> TaskDetails(int id, string returnUrl)
    {
        var model = await this.service.AggregateTask(id);
        if (model == null)
        {
            return this.Redirect(returnUrl);
        }

        model.ReturnUrl = returnUrl;
        return this.View(model);
    }
}
