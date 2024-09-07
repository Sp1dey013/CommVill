using NVelocity.App;
using NVelocity.Runtime.Parser;
using NVelocity;
using CommVill.DAL.Interface;

namespace CommVill.DAL.Helper
{
    public class NVelocityHelper : INVelocityHelper
    {
        private readonly VelocityEngine _velocityEngine;
        public NVelocityHelper()
        {
            _velocityEngine = new VelocityEngine();
            _velocityEngine.Init();
        }
        public async Task<string> MergeOtpBodyAsync(string body, string otp)
        {
            VelocityContext context = new VelocityContext();
            context.Put("OTP", otp);
            StringWriter writer = new StringWriter();
            _velocityEngine.Evaluate(context, writer, "OtpBody", body);
            return await Task.FromResult(writer.ToString());
        }
    }
}
