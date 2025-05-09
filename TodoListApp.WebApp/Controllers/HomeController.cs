using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models.Logging;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controller for managing the home page and user profile views.
    /// </summary>
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">Logger for logging operations in the controller.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Displays the home page view after ensuring the user is authorized.
        /// </summary>
        /// <returns>The view for the home page.</returns>
        [HttpGet("")]
        [HttpGet("Index")]
        [Authorize]
        public IActionResult Index()
        {
            this.logger.LogTrace(nameof(this.Index));

            return this.View();
        }

        /// <summary>
        /// Displays the profile page with user information after ensuring the user is authorized.
        /// </summary>
        /// <returns>The view displaying the user's profile.</returns>
        [HttpGet("Profile")]
        [Authorize]
        public IActionResult Profile()
        {
            this.logger.LogTrace(nameof(this.Profile));

            var email = this.User.FindFirst(ClaimTypes.Email)?.Value!;
            var name = this.User.FindFirst(ClaimTypes.Name)?.Value!;
            var id = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            return this.View((name, email, id));
        }
    }
}
