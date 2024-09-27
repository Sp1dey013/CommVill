using CommVill.Models;

namespace CommVill.DAL.Interface
{
    public interface ICommunityUserRepository
    {
        Task<List<CommunityUser>> GetAllCommunityUserByUserId(Guid user);
        Task<CommunityUser> GetCommunityUserByCommunityUserId(Guid communityUserId);
        Task CreateCommunityUser(CommunityUser communityUser);
        Task UpdateCommunityUser(CommunityUser communityUser);
        Task DeleteCommunityUSerById(Guid communityUserId);
        Task ActiveInactiveCommunityUser(Guid communityId, bool isActive);
        Task<List<CommunityUser>> GetAllCommunityUserByCommunityId(Guid communityId);
    }
}
