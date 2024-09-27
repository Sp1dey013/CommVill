namespace CommVill.Models
{
    public class Post
    {
        public Guid PostId { get; set; }
        public string? PostImage { get; set; }
        public string? PostData { get; set; }
        public long? Like { get; set; }
        public DateTime PostTime { get; set; }
        public Guid CommunityUserId { get; set; }
    }
}
