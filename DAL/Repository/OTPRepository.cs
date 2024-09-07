using CommVill.Controllers;
using CommVill.DAL.Helper;
using CommVill.DAL.Interface;
using CommVill.Models;

namespace CommVill.DAL.Repository
{
    public class OTPRepository : IOTPRepository
    {
        private static Dictionary<string, OTPData> otpDictionary = new Dictionary<string, OTPData>();
        private readonly ILogger<OTP> _logger;
        private readonly EmailRepository _emailRepository;
        private readonly CommVillDBContext _context;
        private readonly NVelocityHelper _nVelocityHelper;
        public OTPRepository(ILogger<OTP> logger,EmailRepository emailRepository,
                               CommVillDBContext context)
        {
            _emailRepository = emailRepository;
            _logger = logger;
            _context = context;
            _nVelocityHelper = new NVelocityHelper();
        }
        public async Task GenerateOTP(string email)
        {
            try
            {
                var emailConfig = await _context.EmailDatas.FindAsync(Guid.Parse("d39417ab-cb1b-4231-987b-9789c4cd571d"));
                Random random = new Random();
                var registrationOtp = random.Next(100000, 999999);
                DateTime otpGenerationTime = DateTime.UtcNow;
                otpDictionary[email] = new OTPData { Email = email, OTP = registrationOtp, OTPGeneratedTime = otpGenerationTime };
                string? body = emailConfig.EmailBodyForOtp;
                var meargedOtpBody = await _nVelocityHelper.MergeOtpBodyAsync(body, registrationOtp.ToString());
                await _emailRepository.SendEmail(email, emailConfig.EmailSubjectForOtp, meargedOtpBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration.");
            }
        }
        public bool ValidateOtp(int userOtp, string email)
        {
            try
            {
                if (otpDictionary.TryGetValue(email, out OTPData? otpData))
                {
                    if (otpData.OTP == userOtp)
                    {
                        TimeSpan timeElapsed = DateTime.UtcNow - otpData.OTPGeneratedTime;
                        if (timeElapsed.TotalMinutes <= 5)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError($"Validate OTP:{e}");
            }
            return false;
        }
        private class OTPData
        {
            public string? Email { get; set; }
            public int? OTP { get; set; }
            public DateTime OTPGeneratedTime { get; set; } = DateTime.UtcNow;
        }
    }
}

