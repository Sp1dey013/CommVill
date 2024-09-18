namespace CommVill.Models
{
    public class Community
    {
        public Guid CommunityId { get; set; }
        public string? Domain { get; set; }
        public string? DomainName { get; set; }
        public string? Description { get; set; }
        public DateTime CommunityCreationTime { get; set; }

    }
    
}
