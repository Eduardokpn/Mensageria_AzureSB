namespace MensageriaProject.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailResendAsync(string to);

        Task SendEmailsAsync(List<string> emails);
    }
}
