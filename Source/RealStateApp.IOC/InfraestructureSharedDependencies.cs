using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Contracts.EmailServices;
using RealEstateApp.Core.Application.Contracts.FileManager;
using RealEstateApp.Core.Domain.Settings.Email;
using RealEstateApp.Infraestructure.Shared.Services.FileManager;
using RealEstateApp.Infrastructure.Shared.Services.Email;

namespace RealStateApp.IOC
{
    public static class InfraestructureSharedDependencies
    {
        public static IServiceCollection AddInfraestructrueShared(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IEmailService, EmailServices>();

            return services;
        }
    }
}
