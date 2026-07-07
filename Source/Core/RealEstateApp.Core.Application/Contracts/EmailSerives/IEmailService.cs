using RealEstateApp.Core.Application.DTOs.Message;

namespace RealEstateApp.Core.Application.Contracts.EmailSerives
{
    public  interface IEmailService
    {
        Task<bool> SendEmailAsync(MessageDto message);
    }
}
