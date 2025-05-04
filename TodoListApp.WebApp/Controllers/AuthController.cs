using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.UserDataAccess;
using TodoListApp.WebApi.Models.DTO;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

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

        var user = await this.userManager.Users
            .FirstOrDefaultAsync(x => EF.Functions.Like(x.Email, registerDTO.Email!));
        if (user == null)
        {
            return this.Unauthorized();
        }

        var result = await this.signInManager.CheckPasswordSignInAsync(user, registerDTO?.Password, false);

        if (!result.Succeeded)
        {
            return this.Unauthorized();
        }

        var token = await this.tokenService.CreateToken(user);
        return this.Ok(token);
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
