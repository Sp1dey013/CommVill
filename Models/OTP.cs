namespace CommVill.Models
{
    public class OTP
    {
        public Guid OTPVerificationID { get; set; }
        public string? Email { get; set; }
        public int? EmailOTP { get; set; }
        public bool? isEmailVerified { get; set; }
        public DateTime OTPCreationTime { get; set; } = DateTime.Now;
    }
}
