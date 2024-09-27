using CommVill.Controllers;
using CommVill.DAL.Interface;
using CommVill.Models;
using System.Data.Entity;

namespace CommVill.DAL.Repository
{
    public class CommunityUserRepository : ICommunityUserRepository
    {
        private readonly CommVillDBContext _context;
        private readonly ILogger<CommunityUserRepository> _logger;

        public CommunityUserRepository(CommVillDBContext context, ILogger<CommunityUserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ActiveInactiveCommunityUser(Guid communityUserId, bool isActive)
        {
            try
            {
                var communityUser = await _context.CommunityUsers.SingleOrDefaultAsync(x => x.CommunityUserId == communityUserId);
                if (communityUser == null)
                {
                    communityUser.IsActive = isActive;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e) { _logger.LogError($"ActiveInactiveCommunityUser: {e}"); }
        }

        public async Task CreateCommunityUser(CommunityUser communityUser)
        {
            try
            {
                communityUser.CommunityUserCreationTime = DateTime.UtcNow;
                await _context.CommunityUsers.AddAsync(communityUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { _logger.LogError($"CreateCommunityUser: {e}"); }
        }

        public async Task DeleteCommunityUSerById(Guid communityUserId)
        {
            try
            {
                CommunityUser? communityUser = await GetCommunityUserByCommunityUserId(communityUserId);
                _context.CommunityUsers.Remove(communityUser);
                await _context.SaveChangesAsync();
            }
            catch(Exception e) { _logger.LogError($"DeleteCommunityUSerById: {e}"); }
        }

        public async Task<List<CommunityUser>> GetAllCommunityUserByCommunityId(Guid communityId)
        {
            try
            {
                return await _context.CommunityUsers.Where(x => x.CommunityId == communityId).ToListAsync();
            }
            catch(Exception e) { _logger.LogError($"GetAllCommunityUserByCommunityId: {e}"); }
            return new List<CommunityUser> { };
        }

        public async Task<List<CommunityUser>> GetAllCommunityUserByUserId(Guid userId)
        {
            try
            {
                return await _context.CommunityUsers.Where(x => x.UserId == userId).ToListAsync();
            }
            catch (Exception e) { _logger.LogError($"GetAllCommunityUserByUserId: {e}"); }
            return new List<CommunityUser> { };
        }

        public async Task<CommunityUser> GetCommunityUserByCommunityUserId(Guid communityUserId)
        {
            try
            {
                return await _context.CommunityUsers.SingleOrDefaultAsync(x => x.CommunityUserId == communityUserId);
            }
            catch(Exception e) { _logger.LogError($"GetCommunityUserByCommunityUserId: {e}"); }
            return new CommunityUser { };
        }

        public async Task UpdateCommunityUser(CommunityUser communityUser)
        {
            try
            {
                _context.CommunityUsers.UpdateRange(communityUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { _logger.LogError($"UpdateCommunityUser: {e}"); }
        }
    }
}
