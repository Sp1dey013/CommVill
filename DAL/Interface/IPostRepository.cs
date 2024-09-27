using CommVill.Models;

namespace CommVill.DAL.Interface
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPostByUserId(Guid userId);
        Task <List<Post>> GetAllPostByCommunityuserId(Guid communityUserId);
        Task<List<Post>> GetallPostByCommuntiyId(Guid communityId);
        Task<Post> GetPostByPostId(Guid postId);
        Task CreatePost(Post post);
        Task UpdatePost(Post post);
        Task DeletePost(Guid postId);
    }
}
