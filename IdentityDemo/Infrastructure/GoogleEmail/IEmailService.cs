namespace IdentityDemoSysteam.Infrastructure.GoogleEmail
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
    }

}
