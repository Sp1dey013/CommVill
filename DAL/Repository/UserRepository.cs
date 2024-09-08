using CommVill.Controllers;
using CommVill.DAL.Interface;
using CommVill.Models;
using Microsoft.AspNetCore.Identity;
using NVelocity.Runtime.Parser;

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

    }
}
