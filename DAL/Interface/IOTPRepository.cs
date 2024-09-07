namespace CommVill.DAL.Interface
{
    public interface IOTPRepository
    {
        Task GenerateOTP(string email);
        bool ValidateOtp(int userOtp, string email);
    }
}
