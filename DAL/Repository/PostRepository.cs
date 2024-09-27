using CommVill.Controllers;
using CommVill.DAL.Interface;
using CommVill.Models;
using System.Data.Entity;

namespace CommVill.DAL.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ILogger<Post> _logger;
        private readonly CommVillDBContext _context;
        private readonly ICommunityRepository _communityRepository;

        public PostRepository(ILogger<Post> logger, CommVillDBContext context, ICommunityRepository communityRepository)
        {
            _logger = logger;
            _context = context;
            _communityRepository = communityRepository;
        }

        public async Task CreatePost(Post post)
        {
            try
            {
                post.PostTime = DateTime.Now;
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { _logger.LogError($"CreatePost: {e}"); }
        }

        public async Task DeletePost(Guid postId)
        {
            try
            {
                Post? post = await GetPostByPostId(postId);
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) { _logger.LogError($"DeletePost: {e}"); }
        }

        public async Task<List<Post>> GetAllPostByCommunityuserId(Guid communityUserId)
        {
            try
            {
                return await _context.Posts.Where(x => x.CommunityUserId == communityUserId).ToListAsync();
            }
            catch(Exception e) { _logger.LogError($"GetAllPostByCommunityuserId: {e}"); }
            return new List<Post> { };
        }

        public async Task<List<Post>> GetallPostByCommuntiyId(Guid communityId)
        {
            try
            {
                List<Guid> communityUserIds = await _communityRepository.GetCommunityUserIdByCommunityId(communityId);
                List<Post> posts = await _context.Posts.Where(x => communityUserIds.Contains(x.CommunityUserId)).ToListAsync();
                return posts;
            }
            catch (Exception e) { _logger.LogError($"GetallPostByCommuntiyId: {e}"); }
            return new List<Post> { };
        }

        public async Task<List<Post>> GetAllPostByUserId(Guid userId)
        {
            List<Guid> communityUserIds = await _communityRepository.GetCommunityUserIdsByUserId(userId);
            List<Post> posts = await _context.Posts.Where(x => communityUserIds.Contains(x.CommunityUserId)).ToListAsync();
            return posts;
        }

        public Task<Post> GetPostByPostId(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
