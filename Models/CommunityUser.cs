namespace CommVill.Models
{
    class CommunityUser
    {
        public Guid CommunityUserId { get; set; }
        public string? CommunityUserName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CommunityUserCreationTime { get; set; }
        public Guid CommunityId { get; set; }
        public Guid UserId { get; set; }

    }
}
