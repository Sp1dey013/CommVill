using CommVill.DAL.Interface;
using CommVill.Models;
using Microsoft.AspNet.Identity;
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

        public async Task<JwtSecurityToken> GenerateJWTToken(string email)
        {
            try
            {
                var authuser = await _userManager.FindByEmailAsync(email);

                if (authuser == null)
                {
                    _logger.LogWarning("User not found with email: {Email}", email);
                    return null;
                }

                var userRoles = await _userManager.GetRolesAsync(authuser);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, authuser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:TokenExpiryMinutes"])), // Configurable expiry
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return token;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while generating the JWT token for email: {Email}", email);
                return null;
            }
        }
    }
}
