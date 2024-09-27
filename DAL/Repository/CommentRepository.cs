using CommVill.Controllers;
using CommVill.DAL.Interface;
using CommVill.Models;
using System.Data.Entity;

namespace CommVill.DAL.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommVillDBContext _context;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(CommVillDBContext context, ILogger<CommentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task createComment(Comment comment)
        {
            try
            {
                comment.CommentTime = DateTime.UtcNow;
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { _logger.LogError($"createComment: {e}"); }
        }

        public async Task deleteComment(Guid commentId)
        {
            try
            {
                Comment? comment = await GetCommentByCommentId(commentId);
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { _logger.LogError($"deleteComment: {e}"); }
        }

        public async Task<List<Comment>> GetAllCommentByPostId(Guid postId)
        {
            try
            {
                return await _context.Comments.Where(x => x.PostId == postId).ToListAsync();
            }
            catch (Exception e) { _logger.LogError($"GetAllCommentByPostId: {e}"); }
            return new List<Comment>();
        }

        public async Task<List<Comment>> GetAllCommentByPrimaryCommentId(Guid primaryCommentId)
        {
            try
            {
                return await _context.Comments.Where(x => x.PrimaryCommentId == primaryCommentId).ToListAsync();
            }
            catch (Exception e) { _logger.LogError($"GetAllCommentByPrimaryCommentId: {e}"); }
            return new List<Comment>();
        }

        public async Task<Comment> GetCommentByCommentId(Guid commentId)
        {
            try
            {
                return await _context.Comments.SingleOrDefaultAsync(x => x.CommnetId == commentId);
            }
            catch (Exception e) { _logger.LogError($"GetCommentByCommentId: {e}"); }
            return new Comment();
        }

        public async Task updateComment(Comment comment)
        {
            try
            {
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
            }
            catch(Exception e) { _logger.LogError($"updateComment: {e}"); }
        }
    }
}
