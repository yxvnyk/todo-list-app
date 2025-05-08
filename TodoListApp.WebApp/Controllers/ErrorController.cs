using System.Net.Sockets;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        private readonly ILogger logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// This action is invoked when an error occurs within the application.
        /// It checks the exception type and returns a corresponding error view.
        /// </summary>
        /// <returns>An appropriate error view based on the exception type.</returns>
        public IActionResult Index()
        {
            LoggerExtensions.LogTrace(this.logger, nameof(this.Index));

            var exception = this.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            ErrorViewModel errorModel = new ErrorViewModel();

            LoggerExtensions.LogError(this.logger, exception?.Message!);

            switch (exception)
            {
                case SocketException:
                    errorModel.ReturnUrl = "/Home/Index";
                    errorModel.Error = "Server error";
                    errorModel.Details = "No connection could be made because the target machine actively refused it.";
                    return this.View("Index", errorModel);

                case HttpRequestException:
                    errorModel.ReturnUrl = "/Home/Index";
                    errorModel.Error = "Server error";
                    errorModel.Details = "No connection could be made because the target machine actively refused it.";
                    return this.View("Index", errorModel);

                case UnauthorizedAccessException:
                    return this.View("~/Views/Auth/AuthMenu.cshtml");

                default:
                    errorModel.ReturnUrl = "/Home/Index";
                    errorModel.Error = "An error has occurred.";
                    return this.View("Index", errorModel);
            }
        }
    }
}
