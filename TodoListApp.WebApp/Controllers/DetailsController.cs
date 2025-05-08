using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Infrastructure;

namespace TodoListApp.WebApp.Controllers;

[Route("details")]
public class DetailsController : Controller
{
    private readonly TaskAggregatorService service;
    private readonly ILogger logger;

    public DetailsController(TaskAggregatorService service, ILogger<DetailsController> logger)
    {
        this.service = service;
        this.logger = logger;
    }

    [Route("task")]
    public async Task<IActionResult> TaskDetails(int id, string returnUrl)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.TaskDetails));

        var model = await this.service.AggregateTask(id);
        if (model == null)
        {
            return this.Redirect(returnUrl);
        }

        model.ReturnUrl = returnUrl;
        var ownerId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        return this.View((model, ownerId == model.OwnerId));
    }
}
