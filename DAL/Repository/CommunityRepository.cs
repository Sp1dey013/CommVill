using CommVill.Controllers;
using CommVill.DAL.Interface;
using System.Data.Entity;
using CommVill.Models;

namespace CommVill.DAL.Repository
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ILogger<CommunityRepository> _logger;
        private readonly CommVillDBContext _context;

        public CommunityRepository(ILogger<CommunityRepository> logger, CommVillDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task ActiveInactiveCommunity(Guid communityId, bool isActive)
        {
            try
            {
                var community = await _context.Communities.SingleOrDefaultAsync(x => x.CommunityId == communityId);
                if (community == null)
                {
                    community.IsActive = isActive;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e) { _logger.LogError($"ActiveInactiveCommunity: {e}"); }
        }

        public async Task CreateCommunity(Community community)
        {
            try
            {
                community.CommunityCreationTime = DateTime.UtcNow;
                await _context.Communities.AddAsync(community);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { _logger.LogError($"CreateCommunity: {e}"); }
        }

        public async Task DeleteCommunity(Guid communityId)
        {
            try
            {
                Community? community = await GetCommunityByCommunityId(communityId);
                _context.Communities.Remove(community);
                await _context.SaveChangesAsync();
            }
            catch(Exception e) { _logger.LogError($"DeleteCommunity: {e}"); }
        }

        public async Task<List<Community>> GetAllCommunity()
        {
            try
            {
                return await _context.Communities.ToListAsync();
            }
            catch (Exception e) { _logger.LogError($"GetAllCommunity: {e}"); }
            return new List<Community> { };
        }

        public async Task<List<Guid>> GetCommunityUserIdByCommunityId(Guid communityId)
        {
            return await _context.CommunityUsers.Where(x => x.CommunityId == communityId).Select(x=>x.CommunityUserId).ToListAsync();
        }

        public async Task<Community> GetCommunityByCommunityId(Guid communityId)
        {
            try
            {
                return await _context.Communities.SingleOrDefaultAsync(x => x.CommunityId == communityId);
            }
            catch (Exception e) { _logger.LogError($"GetCommunityByCommunityId: {e}"); }
            return new Community { };
        }

        public Task<List<Community>> GetCommunitybyUserId(Guid UserId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCommunity(Community community)
        {
            try
            {
                _context.Communities.UpdateRange(community);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            { _logger.LogError($"UpdateCommunity: {e}"); }
        }

        public Task<List<Guid>> GetCommunityUserIdsByUserId(Guid userId)
        {
            return _context.CommunityUsers.Where(x=>x.UserId == userId).Select(x=>x.CommunityUserId).ToListAsync();
        }
    }
}
