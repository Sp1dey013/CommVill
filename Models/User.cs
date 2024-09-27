using System.ComponentModel.DataAnnotations;

namespace CommVill.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime UserCreationTime { get; set; }
        public bool? IsActive { get; set; }
        public string? Reason { get; set; }
    }
}
