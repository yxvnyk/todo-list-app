using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.tokenService = tokenService;
    }

    [HttpGet("signout")]
    public new async Task<IActionResult> SignOut()
    {
        await this.HttpContext.SignOutAsync(
        IdentityConstants.ApplicationScheme);

        return this.RedirectToAction("login");
    }

    [HttpGet("login")]
    public IActionResult LogIn()
    {
        return this.View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LogIn(LoginDTO registerDTO)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View();
        }

        var email = registerDTO.Email.ToLower();
        var user = await this.userManager.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

        if (user == null)
        {
            return this.Unauthorized();
        }

        var result = await this.signInManager.CheckPasswordSignInAsync(user, registerDTO?.Password, false);

        if (!result.Succeeded)
        {
            return this.Unauthorized();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Email),
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
            //AllowRefresh = <bool>,
            // Refreshing the authentication session
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            IsPersistent = true,
            //IssuedUtc = <DateTimeOffset>
            //RedirectUri = <string>
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
        return this.View("~/Views/Home/Index.cshtml");
    }

    [HttpGet("signin")]
    public IActionResult SignIn()
    {
        return this.View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO register)
    {
        if (!this.ModelState.IsValid)
        {
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

            return this.View();
        }

        return this.View();
    }
}
