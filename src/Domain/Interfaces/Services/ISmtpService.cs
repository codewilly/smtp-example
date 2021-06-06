using Domain.Commands;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface ISmtpService
    {
        Task<bool> SendEmailAsync(SendEmailCommand command);
    }
}
