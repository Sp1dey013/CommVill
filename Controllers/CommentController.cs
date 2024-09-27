using CommVill.DAL.Interface;
using CommVill.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommVill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly ICommentRepository _commentRepository;

        public CommentController(ILogger<CommentController> logger, ICommentRepository commentRepository)
        {
            _logger = logger;
            _commentRepository = commentRepository;
        }
        [HttpPost]
        public async Task<ActionResult> CreateComment(Comment comment, Guid postId)
        {
            try
            {
                await _commentRepository.createComment(comment);
                return Ok();
            }
            catch (Exception e) { _logger.LogError($"CreateComment: {e}"); }
            return BadRequest();
        }
    }
}
