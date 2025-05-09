using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.UserDataAccess.Entity;
using TodoListApp.WebApi.Models.DTO;
using TodoListApp.WebApi.Models.Logging;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers
{
    /// <summary>
    /// Controller for managing authentication and user registration/login.
    /// </summary>
    [AllowAnonymous] // Allow access without authentication for the actions in this controller
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userManager">User manager for managing users.</param>
        /// <param name="signInManager">Sign-in manager for handling user sign-in operations.</param>
        /// <param name="tokenService">Service for generating JWT tokens.</param>
        /// <param name="logger">Logger instance for logging actions and errors.</param>
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, ILogger<AuthController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.logger = logger;
        }

        /// <summary>
        /// Logs out the current user and redirects to the AuthMenu view.
        /// </summary>
        [HttpGet("logout")]
        public async Task<IActionResult> LogOut()
        {
            this.logger.LogTrace(nameof(this.LogOut));

            await this.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            return this.View("AuthMenu");
        }

        /// <summary>
        /// Returns the login view.
        /// </summary>
        [HttpGet("login")]
        public IActionResult LogIn()
        {
            return this.View();
        }

        /// <summary>
        /// Logs in the user using the provided login credentials (email and password).
        /// </summary>
        /// <param name="registerDTO">The login data transfer object containing email and password.</param>
        /// <returns>Redirects to LogInComplete if successful, otherwise returns an error message.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LoginDto registerDTO)
        {
            this.logger.LogTrace(nameof(this.LogIn));

            if (!this.ModelState.IsValid)
            {
                this.logger.LogWarning("Invalid ModelState");
                return this.View();
            }

            var user = await this.userManager.Users
             .FirstOrDefaultAsync(x =>
                 EF.Functions.Collate(x.Email, "Latin1_General_CI_AS") == registerDTO.Email);

            if (user == null)
            {
                this.logger.LogWarning("Wrong email");
                return this.View("Login", "Wrong email");
            }

            var result = await this.signInManager.CheckPasswordSignInAsync(user, registerDTO?.Password, false);

            if (!result.Succeeded)
            {
                this.logger.LogWarning("Wrong password");
                return this.View("Login", "Wrong password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var userRoles = await this.userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, IdentityConstants.ApplicationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true,
            };

            await this.HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            var token = await this.tokenService.CreateToken(user);
            this.HttpContext.Session.SetString("JwtToken", token);

            return this.RedirectToAction("LogInComplete");
        }

        /// <summary>
        /// Completes the login process and redirects to the home page.
        /// </summary>
        [HttpGet("complete")]
        public IActionResult LogInComplete()
        {
            this.logger.LogTrace(nameof(this.LogInComplete));

            return this.View("~/Views/Home/Index.cshtml");
        }

        /// <summary>
        /// Returns the sign-in view where the user can register.
        /// </summary>
        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            this.logger.LogTrace(nameof(this.SignIn));

            return this.View();
        }

        /// <summary>
        /// Registers a new user with the provided registration data.
        /// </summary>
        /// <param name="register">The registration data transfer object containing username, email, and password.</param>
        /// <returns>Redirects to the login view upon successful registration, otherwise shows an error message.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            this.logger.LogTrace(nameof(this.Register));

            if (!this.ModelState.IsValid)
            {
                this.logger.LogWarning("Invalid ModelState");
                return this.View();
            }

            var appUser = new AppUser()
            {
                UserName = register?.Username,
                Email = register?.Email,
            };

            var createUser = await this.userManager.CreateAsync(appUser, register?.Password);

            if (createUser.Succeeded)
            {
                var roleResult = await this.userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    return this.RedirectToAction("Login");
                }

                this.logger.LogWarning("Role adding failed");
                return this.View();
            }

            this.logger.LogWarning("User creation failed");
            return this.View();
        }
    }
}
