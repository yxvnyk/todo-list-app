using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.UserDataAccess;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration configuration;

    private readonly SymmetricSecurityKey symmetricSecurityKey;

    private readonly UserManager<AppUser> userManager;

    public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        this.configuration = configuration;
        this.symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(this.configuration["JWT:SigningKey"]));
        this.userManager = userManager;
    }

    public async Task<string> CreateToken(AppUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
        };

        var creds = new SigningCredentials(this.symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

        var userRoles = await this.userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds,
            Issuer = this.configuration["JWT:Issuer"],
            Audience = this.configuration["JWT:Audience"],
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
