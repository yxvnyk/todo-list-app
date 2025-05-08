using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Infrastructure;

namespace TodoListApp.WebApp.Controllers
{
    /// <summary>
    /// Controller responsible for displaying task details.
    /// </summary>
    [Route("details")]
    public class DetailsController : Controller
    {
        private readonly TaskAggregatorService service;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsController"/> class.
        /// </summary>
        /// <param name="service">The service used to aggregate task details.</param>
        /// <param name="logger">Logger instance for logging actions and errors.</param>
        public DetailsController(TaskAggregatorService service, ILogger<DetailsController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Displays detailed information for a specific task.
        /// </summary>
        /// <param name="id">The ID of the task to fetch details for.</param>
        /// <param name="returnUrl">The URL to which the user should be redirected if the task is not found.</param>
        /// <returns>The view displaying task details if found, otherwise redirects to the returnUrl.</returns>
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
}
