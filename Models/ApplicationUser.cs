
using Microsoft.AspNetCore.Identity;

namespace CommVill.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Errors { get; set; }
    }
}
