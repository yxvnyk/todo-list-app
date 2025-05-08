using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[Route("")]
[Route("Home")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    [Authorize]
    public IActionResult Index()
    {
        return this.View();
    }

    [HttpGet("Profile")]
    [Authorize]
    public IActionResult Profile()
    {
        var email = this.User.FindFirst(ClaimTypes.Email)?.Value!;
        var name = this.User.FindFirst(ClaimTypes.Name)?.Value!;
        var id = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        return this.View((name, email, id));
    }

    //  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //  public IActionResult Error()
    //  {
    //      return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    //  }
}
