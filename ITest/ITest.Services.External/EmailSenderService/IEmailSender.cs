using System.Threading.Tasks;

namespace ITest.Services.External.EmailSenderService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
