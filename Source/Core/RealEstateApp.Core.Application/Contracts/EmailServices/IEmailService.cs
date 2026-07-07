using RealEstateApp.Core.Application.DTOs.Message;

namespace RealEstateApp.Core.Application.Contracts.EmailServices
{
    public  interface IEmailService
    {
        Task<bool> SendEmailAsync(MessageDto message);
    }
}
