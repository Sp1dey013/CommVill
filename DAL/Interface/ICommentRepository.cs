using CommVill.Models;

namespace CommVill.DAL.Interface
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentByPostId(Guid postId);
        Task<List<Comment>> GetAllCommentByPrimaryCommentId(Guid primaryCommentId);
        Task<Comment> GetCommentByCommentId(Guid commentId);
        Task createComment(Comment comment);
        Task updateComment(Comment comment);
        Task deleteComment(Guid commentId);
    }
}
