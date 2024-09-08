namespace CommVill.Models
{
    class CommunityUser
    {
        public Guid CommunityUserId { get; set; }
        public string? CommunityUserName { get; set; }
        public Guid CommunityId { get; set; }
        public Guid UserId { get; set; }

    }
}
