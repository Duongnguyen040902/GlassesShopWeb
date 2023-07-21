namespace DemoWebFirstMVCCore
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email,string message);
        Task SendEmailOrderAsync(string email);
    }
}
