namespace CommVill.Models
{
    public class EmailData
    {
        public Guid EmailDataId { get; set; }
        public string? EmailSubjectForOtp { get; set; }
        public string? EmailBodyForOtp { get; set; }
    }
}
