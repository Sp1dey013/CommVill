namespace CommVill.Models
{
    public class Community
    {
        public Guid CommunityId { get; set; }
        public string? Domain { get; set; }
        public string? CommunityName { get; set; }
        public string? Description { get; set; }
        public DateTime CommunityCreationTime { get; set; }
        public bool? IsActive { get; set; }
        public string? Reason { get; set; }
    }
    
}
