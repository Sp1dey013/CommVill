using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CommVill.Models;

namespace CommVill.Controllers
{
    public class CommVillDBContext : IdentityDbContext<ApplicationUser>
    {
        public CommVillDBContext(DbContextOptions<CommVillDBContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<EmailConfig> EmailConfigs { get; set; }
        public DbSet<EmailData> EmailDatas { get; set; }
        


    }
}
