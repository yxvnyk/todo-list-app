using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.UserDataAccess.Entity;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services
{
    /// <summary>
    /// Service for handling JWT token creation for authenticated users.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        private readonly SymmetricSecurityKey symmetricSecurityKey;

        private readonly UserManager<AppUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The application's configuration to read JWT settings.</param>
        /// <param name="userManager">The <see cref="UserManager{AppUser}"/> for accessing user roles.</param>
        public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            this.configuration = configuration;
            this.symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(this.configuration["JWT:SigningKey"]));
            this.userManager = userManager;
        }

        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token will be created.</param>
        /// <returns>A task that represents the asynchronous operation, with the JWT token string as the result.</returns>
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
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = creds,
                Issuer = this.configuration["JWT:Issuer"],
                Audience = this.configuration["JWT:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
