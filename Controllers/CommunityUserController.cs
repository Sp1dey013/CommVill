using CommVill.DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommVill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityUserController : ControllerBase
    {
        private readonly ILogger<CommunityUserController> _logger;
        private readonly ICommunityRepository _communityRepository;

        public CommunityUserController(ILogger<CommunityUserController> logger, ICommunityRepository communityRepository)
        {
            _logger = logger;
            _communityRepository = communityRepository;
        }
        [HttpGet("GetAllCommunityUserByUserId")]
        public async Task<List<CommunityUser> GetAllCommunityUserForUser(Guid userId)
        {
            try
            {
                return await _communityRepository.GetAllCommunityUserByUserId(userId);
            }
            catch(Exception e){_logger.logError($"GetAllCommunityUserForUser: {e}");}
            return new List<CommunityUser>{};
        }
        [HttpGet("GetAllCommunityUserBy")]
    }
}
