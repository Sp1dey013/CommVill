namespace CommVill.Models
{
    public class Post
    {
        public Guid PostId { get; set; }
        public string? PostData { get; set; }
        public long Like { get; set; }
        public Guid CommunityUserId { get; set; }
        public Guid UserId { get; set; }
    }
    class Comment
    {
        public Guid CommnetId { get; set; }
        public string? CommentData { get; set; }
        public Guid PostId { get; set; }
        public Guid PrimaryCommentId { get; set; }
    }
}
