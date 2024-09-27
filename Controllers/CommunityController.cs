using CommVill.DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommVill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ILogger<CommunityController> _logger;
        private readonly ICommunityRepository _communityRepository;

        public CommunityController(ILogger<CommunityController> logger, ICommunityRepository communityRepository)
        {
            _logger = logger;
            _communityRepository = communityRepository;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCommunity(Guid communityId)
        {
            try
            {
                await _communityRepository.DeleteCommunity(communityId);
                return Ok();
            }
            catch (Exception e) { _logger.LogError($"DeleteCommunity: {e}"); }
            return BadRequest();
        }
    }
}
