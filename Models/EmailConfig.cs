using System.ComponentModel.DataAnnotations;

namespace CommVill.Models
{
    public class EmailConfig
    {
        [Key]
        public Guid EmailConfigId { get; set; }
        public string? SmtpServer { get; set; }
        public int? SmtpPort { get; set; }
        public string? EmailCC { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? Active { get; set; }
    }
}
