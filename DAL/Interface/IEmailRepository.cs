namespace CommVill.DAL.Interface
{
    public interface IEmailRepository
    {
        Task SendEmail(string email, string body, string subject);
    }
}
