using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using CommVill.Controllers;
using CommVill.Models;
using Microsoft.AspNetCore.Identity;
using CommVill.DAL.Interface;

namespace CommVill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
        private readonly IOTPRepository _otpVerification;
        private readonly ILogger<OTPController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public OTPController(IOTPRepository generateOTP, IConfiguration configuration,
                                         ILogger<OTPController> logger, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _logger = logger;
            _otpVerification = generateOTP;
            _userManager = userManager;
        }
        [HttpPost("OTP-Generate")]
        public async Task<ActionResult<string>> GenerateOTP(string email)
        {
            try
            {
                var blockedDomains = _configuration.GetSection("BlockedDomains").Get<List<string>>();
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email address is required.");
                }
                if (existingUser != null)
                {
                    return BadRequest("Email already exist.");
                }
                await _otpVerification.GenerateOTP(email);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during OTP generation.");
            }
            return BadRequest();
        }
        [HttpPost("OTP-validate")]
        public ActionResult ValidateOtp(int userOtp, string email)
        {
            try
            {
                bool isValid = _otpVerification.ValidateOtp(userOtp, email);
                if (isValid)
                {
                    return Ok("OTP is valid.");
                }
                return BadRequest("Invalid OTP or OTP expired.");
            }
            catch (Exception e)
            {
                _logger.LogError($"ValidateOtp:{e}");
            }
            return BadRequest();
        }
    }
}
