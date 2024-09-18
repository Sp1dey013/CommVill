namespace CommVill.Models
{
    public class PostText
    {
        public Guid PostTextId { get; set; }
        public string? PostTextData { get; set; }
        public long Like { get; set; }
        public DateTime PostTime { get; set; }
        public Guid CommunityUserId { get; set; }
        public Guid UserId { get; set; }
    }
    public class PostImage
    {
        public Guid PostImageId { get; set; }
        public string? PostTextData { get; set; }
        public long Like { get; set; }
        public DateTime PostTime { get; set; }
        public Guid CommunityUserId { get; set; }
        public Guid UserId { get; set; }
    }
    
}
