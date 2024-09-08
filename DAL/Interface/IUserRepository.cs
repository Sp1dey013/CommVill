using CommVill.Models;

namespace CommVill.DAL.Interface
{
    public interface IUserRepository
    {
        Task<ApplicationUser> CreateNewUser(User user);
    }
}
