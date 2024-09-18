using CommVill.DAL.Helper;
using CommVill.DAL.Interface;
using CommVill.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NVelocity.Runtime.Parser;
using System.IdentityModel.Tokens.Jwt;

namespace CommVill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailRepository _emailRepository;
        private readonly INVelocityHelper _nVelocityHelper;
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository,
            ILogger<AuthController> logger,
            RoleManager<IdentityRole> roleManager,
            IEmailRepository emailRepository,
            INVelocityHelper nVelocityHelper,
            IAuthRepository authRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _emailRepository = emailRepository;
            _nVelocityHelper = nVelocityHelper;
            _authRepository = authRepository;
            _userRepository = userRepository;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> VerifyLoginUser(Login login)
        {
            try
            {
                ApplicationUser? authUser = await _userManager.FindByEmailAsync(login.Email);
                if (authUser == null)
                {
                    return BadRequest("Invalid user");
                }

                if (!await _userManager.CheckPasswordAsync(authUser, login.Password))
                {
                    return BadRequest("Invalid email or password.");
                }

                var token = await _authRepository.GenerateJWTToken(authUser.Email);
                if (token != null)
                {
                    _logger.LogInformation($"User login successfully {login.Email}");
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during VerifyLoginUser: {ex.Message}");
            }
            return BadRequest();
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return BadRequest("Email already exists");
                }
                ApplicationUser newUser = await _userRepository.CreateNewUser(user);
                if (newUser == null)
                {
                    return BadRequest("User creation failed");
                }
                if (!string.IsNullOrEmpty(newUser.Errors))
                {
                    return BadRequest(newUser.Errors);
                }
                await _userManager.AddToRoleAsync(newUser,"User");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to register User. Exception: {ex}");
            }
            return BadRequest();
        }
    }
}
