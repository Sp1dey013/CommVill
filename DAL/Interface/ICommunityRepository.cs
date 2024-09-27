using CommVill.Models;

namespace CommVill.DAL.Interface
{
    public interface ICommunityRepository
    {
        Task<List<Community>> GetAllCommunity();
        Task<List<Community>> GetCommunitybyUserId(Guid UserId);
        Task<Community> GetCommunityByCommunityId(Guid CommunityId);
        Task CreateCommunity(Community community);
        Task UpdateCommunity(Community community);
        Task DeleteCommunity(Guid CommunityId);
        Task ActiveInactiveCommunity(Guid CommunityId,bool isActive);
        Task<List<Guid>> GetCommunityUserIdByCommunityId(Guid communityId);
        Task<List<Guid>> GetCommunityUserIdsByUserId(Guid userId);

    }
}
