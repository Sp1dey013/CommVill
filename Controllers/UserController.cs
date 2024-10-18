using CommVill.DAL.Interface;
using CommVill.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommVill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        [HttpGet("GetAllUser")]
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                return await _userRepository.GetAllUsers();
            }
            catch (Exception e) { _logger.LogError("Error occurred while Getting all user"); }
            return new List<User>();
        }
        [HttpGet("GetUser")]
        public async Task<User?> GetUserByUserId(Guid userId )
        {
            try
            {
                return await _userRepository.GetUserByUserId(userId);
            }
            catch (Exception x) { _logger.LogError($"Error occurred while getting user details by Id{userId}."); }
            return new User { };
        }
        [HttpPut("UpdateUser")]
        public async Task Updateuser(User user)
        {
            try
            {
                await _userRepository.UpdateUser(user);
            }
            catch (Exception e) { _logger.LogError($"Error occurred while updating user: {user.Email}."); }
        }
        [HttpDelete("DeleteUser")]
        public async void Deleteuser(Guid userId)
        {
            try
            {
                await _userRepository.DeleteUser(userId);
            }
            catch(Exception e) { _logger.LogError($"{e}"); }
        }
        [HttpPost("ActiveInactiveUser")]
        public async Task ActiveInactiveUser(string emial, bool isActive)
        {
            try
            {
                await _userRepository.ActiveInactiveUser(emial, isActive);
            }
            catch (Exception e) { _logger.LogError($"{e}"); }
        }
        [HttpPost("ChangeUserPassword")]
        public async Task<IActionResult> ChangeUserPassword(string email, string oldPassword,string newPassword)
        {
            try
            {
                var user = await _userRepository.GetUserDataByEmail(email);
                var isPasswordValid = await _userRepository.CheckUserPassword(user, oldPassword);
                if (isPasswordValid != true)
                {
                    return BadRequest("Old password is invalid");
                }
                await _userRepository.ChangeUserPassword(user, oldPassword, newPassword);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError($"An error occured while changeing password: {e}.");
            }
            return BadRequest();
        }
        [HttpGet("GetUserIdByEmail")]
        public async Task<Guid> GetUserIdByEmail(string email)
        {
            try
            {
                return await _userRepository.GetUserIdByEmail(email);
            }
            catch (Exception e)
            {
                _logger.LogError($"GetUserIdByEmail{e}");
            }
            return Guid.Empty;
        }
    }
}
