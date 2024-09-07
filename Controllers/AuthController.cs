using CommVill.DAL.Helper;
using CommVill.DAL.Interface;
using CommVill.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NVelocity.Runtime.Parser;

namespace CommVill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly CommVillDBContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailRepository _emailRepository;
        private readonly INVelocityHelper _nVelocityHelper;
        private readonly IAuthRepository _authRepository;
        
        public AuthController(UserManager<ApplicationUser> userManager, ILogger<AuthController> logger, CommVillDBContext context, RoleManager<IdentityRole> roleManager, IEmailRepository emailRepository, INVelocityHelper nVelocityHelper, IAuthRepository authRepository)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
            _emailRepository = emailRepository;
            _nVelocityHelper = nVelocityHelper;
            _authRepository = authRepository;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> VerifyLoginUser(Login login)
        {
            try
            {
                var authUser = await _userManager.FindByEmailAsync(login.Email);
                if (authUser == null)
                {
                    return BadRequest("Invalid user");
                }
                var role = await _userManager.GetRolesAsync(authUser);
                //if (role.Contains("Partner"))
                //{
                //    bool isPartnerActive = await _partnerRepository.IsPartnerActive(login.Email);
                //    if (!isPartnerActive)
                //    {
                //        return Unauthorized("Unverified Partner");
                //    }
                //}
                if (!await _userManager.CheckPasswordAsync(authUser, login.Password))
                {
                    return BadRequest("Invalid email or password.");
                }
                var token = await _authRepository.GenerateJWTToken(authUser.Email);
                if (token != null)
                {
                    //_cookieRepository.SetCookie("AuthToken", token.ToString(), 1);
                    _logger.LogInformation("User login successfully", login.Email);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during VerifyLoginUser:{ex.Message}");
            }
            return BadRequest();
        }
        [HttpPost("register-partner")]
        public async Task<IActionResult> RegisterPartner(Partner partner)
        {
            try
            {
                if (await _partnerRepository.BlockDomain(partner.Email) == true)
                {
                    return BadRequest("Invalid domain name");
                }
                var existingUser = await _userManager.FindByEmailAsync(partner.Email);
                if (existingUser != null)
                {
                    return BadRequest("Email already exist");
                }
                ApplicationUser newUser = await _partnerRepository.CreateNewUser(partner);
                if (!string.IsNullOrEmpty(newUser.Errors))
                {
                    return BadRequest(newUser.Errors);
                }
                if (await _roleManager.RoleExistsAsync(Roles.Partner))
                {
                    await _userManager.AddToRoleAsync(newUser, Roles.Partner);
                }
                else
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Partner));
                    await _userManager.AddToRoleAsync(newUser, Roles.Partner);
                }
                var emailConfig = await _context.EmailConfigs.FindAsync(Guid.Parse("d39417ab-cb1b-4231-987b-9789c4cd571d"));
                if (emailConfig != null)
                {
                    var meargedPartnerBody = await _nVelocityHelper.MergePartnerBodyAsync(emailConfig.EmailBodyForRegisterPartnerSuccessfully, partner);
                    await _emailRepository.SendEmail(partner.Email, emailConfig.EmailSubjectForRegisterPartnerSuccessfully, meargedPartnerBody);
                    _logger.LogInformation("Register Partner Mail send successfully.", partner.Email);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to register partner. Exception: {ex}");

            }
            return BadRequest();
        }
    }
}
