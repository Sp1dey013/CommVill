namespace CommVill.Models
{
    public class Community
    {
        public Guid CommunityId { get; set; }
        public string? Domain { get; set; }
        public string? DomainName { get; set; }
        public string? Description { get; set; }
    }
    class CommunityUser
    {
        public Guid CommunityUserId { get; set; }
        public string? CommunityUserName { get; set; }
        public Guid CommunityId { get; set;}
        public Guid UserId { get; set;}
        
    }
}
