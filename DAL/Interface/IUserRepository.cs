using CommVill.Models;

namespace CommVill.DAL.Interface
{
    public interface IUserRepository
    {
        Task<ApplicationUser> CreateNewUser(User user);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserByUserId(Guid userId);
        Task UpdateUser(User user);
        Task DeleteUser(Guid userId);
        Task ActiveInactive(string email, bool isActive);
        Task ChangeUserPassword(ApplicationUser user, string oldPassword, string newPassword);
        Task<bool> CheckUserPassword(ApplicationUser user, string password);
        Task<ApplicationUser> GetUserDataByEmail(string email);
        Task<Guid> GetUserIdByEmail(string email);
    }
}
