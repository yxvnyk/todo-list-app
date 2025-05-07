using System.Net.Sockets;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers;

[Route("Error")]
public class ErrorController : Controller
{
    public IActionResult Index()
    {
        var exception = this.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        ErrorViewModel errorModel = new ErrorViewModel();

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
                errorModel.Error = "An error occurred.";
                return this.View("Index", errorModel);
        }
    }
}
