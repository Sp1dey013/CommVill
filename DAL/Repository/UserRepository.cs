using CommVill.Controllers;
using CommVill.DAL.Interface;
using CommVill.Models;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;

namespace CommVill.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CommVillDBContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserManager<ApplicationUser> userManager, CommVillDBContext context, ILogger<UserRepository> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<ApplicationUser?> CreateNewUser(User user)
        {
            try
            {
                ApplicationUser newUser = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = user.Email,
                    Email = user.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var createUserResult = await _userManager.CreateAsync(newUser, user.Password);
                if (!createUserResult.Succeeded)
                {
                    newUser.Errors = string.Join(',', createUserResult.Errors);
                }
                else
                {
                    user.UserId = new Guid(newUser.Id);
                    user.UserCreationTime = DateTime.UtcNow;
                    user.Password = null;
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                }
                _logger.LogInformation($"User Created successfully : {user.Email}");
                return newUser;
            }
            catch (Exception e)
            {
                _logger.LogError($"CreateUser : {e}");
            }
            return null;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();

        }

#pragma warning disable CS8613
        public async Task<User?> GetUserByUserId(Guid userId)
#pragma warning restore CS8613
        {
            try
            {
                return await _context.Users.SingleOrDefaultAsync(x => x.UserId == userId);
            }
            catch (Exception e) { _logger.LogError($"Error occurred while getting user: {e}"); }
            return null;
        }
        public async Task UpdateUser(User user)
        {
            try
            {
                _context.Users.UpdateRange(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { _logger.LogError($"Error occurred while updateing user: {e}"); }
        }
        public async Task DeleteUser(Guid userId)
        {
            try
            {
                User? user = await GetUserByUserId(userId);
                var authUser = await _userManager.FindByIdAsync(userId.ToString());
                _context.Users.Remove(user);
                await _userManager.DeleteAsync(authUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { _logger.LogError($"e"); }
        }
        public async Task ActiveInactiveUser(string email, bool isActive)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user != null)
                {
                    user.IsActive = isActive;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning($"User with email {email} not found.");
                }
            }
            catch(Exception e) { _logger.LogError($"{e}"); }
        }
        public async Task ChangePassword(ApplicationUser user, string oldPassword, string newPassword)
        {
            try
            {
                var changePassword = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                await _context.SaveChangesAsync();
            }
            catch (Exception e){ _logger.LogError($"ChangePassword{e}"); }
        }
        public async Task<bool> CheckUserPassword(ApplicationUser user, string password)
        {
            try
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
                if (isPasswordValid)
                { return true; }
                return false;
            }
            catch (Exception e) { _logger.LogError($"ChangeUserPassword{e}"); }
            return false;
        }
        public async Task ChangeUserPassword(ApplicationUser user, string oldPassword, string newPassword)
        {
            try
            {
                var changePassword = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"ChangeUserPassword{e}");
            }
        }
        public async Task<ApplicationUser> GetUserDataByEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError($"GetUserDatabyEmail{e}");
            }
            return new ApplicationUser();
        }

        public async Task<Guid> GetUserIdByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email).Select(x => x.UserId).FirstOrDefaultAsync();
        }
    }
}
