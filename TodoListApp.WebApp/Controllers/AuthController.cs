using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.UserDataAccess;
using TodoListApp.WebApi.Models.DTO;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

[AllowAnonymous]
[Route("Auth")]
public class AuthController : Controller
{
    private readonly UserManager<AppUser> userManager;
    private readonly SignInManager<AppUser> signInManager;
    private readonly ITokenService tokenService;
    private readonly ILogger logger;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, ILogger<AuthController> logger)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.tokenService = tokenService;
        this.logger = logger;
    }

    [HttpGet("logout")]
    public async Task<IActionResult> LogOut()
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.LogOut));

        await this.HttpContext.SignOutAsync(
        IdentityConstants.ApplicationScheme);

        return this.View("AuthMenu");
    }

    [HttpGet("login")]
    public IActionResult LogIn()
    {
        return this.View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LogIn(LoginDTO registerDTO)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.LogIn));

        if (!this.ModelState.IsValid)
        {
            LoggerExtensions.LogWarning(this.logger, "Invalid ModelState");
            return this.View();
        }

        var email = registerDTO?.Email?.ToLower(System.Globalization.CultureInfo.CurrentCulture);
        var user = await this.userManager.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

        if (user == null)
        {
            LoggerExtensions.LogWarning(this.logger, "Wrong email");
            return this.View("Login", "Wrong email");
        }

        var result = await this.signInManager.CheckPasswordSignInAsync(user, registerDTO?.Password, false);

        if (!result.Succeeded)
        {
            LoggerExtensions.LogWarning(this.logger, "Wrong password");
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
            // AllowRefresh = <bool>,
            // Refreshing the authentication session
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            IsPersistent = true,

            // IssuedUtc = <DateTimeOffset>
            // RedirectUri = <string>
        };

        await this.HttpContext.SignInAsync(
            IdentityConstants.ApplicationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        var token = await this.tokenService.CreateToken(user);
        this.HttpContext.Session.SetString("JwtToken", token);

        return this.RedirectToAction("LogInComplete");
    }

    [HttpGet("complete")]
    public IActionResult LogInComplete()
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.LogInComplete));

        return this.View("~/Views/Home/Index.cshtml");
    }

    [HttpGet("signin")]
    public IActionResult SignIn()
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.SignIn));

        return this.View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO register)
    {
        LoggerExtensions.LogTrace(this.logger, nameof(this.Register));

        if (!this.ModelState.IsValid)
        {
            LoggerExtensions.LogWarning(this.logger, "Invalid ModelState");
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

            LoggerExtensions.LogWarning(this.logger, "Role adding faild");
            return this.View();
        }

        LoggerExtensions.LogWarning(this.logger, "User creating faild");
        return this.View();
    }
}
