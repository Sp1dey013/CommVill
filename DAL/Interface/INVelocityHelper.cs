using CommVill.Models;
using NVelocity.Runtime.Parser;

namespace CommVill.DAL.Interface
{
    public interface INVelocityHelper
    {
        Task<string> MergeOtpBodyAsync(string emailBody, string otp);
    }
}
