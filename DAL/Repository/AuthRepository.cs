using CommVill.DAL.Interface;
using CommVill.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CommVill.DAL.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(IConfiguration configuration, UserManager<ApplicationUser> userManager, ILogger<AuthRepository> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<JwtSecurityToken?> GenerateJWTToken(string email)
        {
            try
            {
                var authUser = await _userManager.FindByEmailAsync(email);
                if (authUser == null)
                {
                    _logger.LogWarning($"User not found with email: {email}");
                    return null;
                }
                var userRoles = await _userManager.GetRolesAsync(authUser);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, authUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                if (!double.TryParse(_configuration["JWT:TokenExpiryMinutes"], out double tokenExpiryMinutes))
                {
                    tokenExpiryMinutes = 5;
                }
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddMinutes(tokenExpiryMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                return token;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while generating the JWT token for email: {email}");
                return null;
            }
        }
    }
}
