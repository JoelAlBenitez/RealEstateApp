using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.Contracts.Users.Validation;
using RealEstateApp.Core.Domain.Settings.Email;
using RealEstateApp.Core.Domain.Settings.JWT;
using RealEstateApp.Infraestructure.Identity.Interfaces;
using RealEstateApp.Infraestructure.Identity.Services.ExternalUsers;
using RealEstateApp.Infraestructure.Identity.Services.GenerateTokens;
using RealEstateApp.Infraestructure.Identity.Services.InternalUsers;
using RealEstateApp.Infraestructure.Identity.Services.Validate;

namespace RealEstateApp.Infraestructure.Identity.RegistrationAndConfiguration
{
    public static class IdentityDependencies
    {
        #region dependencies 

        public static IServiceCollection AddDependenciesWebApp(this IServiceCollection services)
        {
            services.AddScoped<IAuthProcesssAccountWebApp, AuthServicesProcccessAccountWebApp>();
            services.AddScoped<IOperationalAccountWebApp, OperationalAccountWebApp>();
            return services;
        }
        public static IServiceCollection AddDependenciesCommon(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IGenerateTokens, GenerateTokens>();
            services.AddScoped<IServicesValidateUsers, ServicesValidateUsers>();
            return services;
        }
        public static IServiceCollection AddDependenciesWebApi(this IServiceCollection services)
        {
            services.AddScoped<IAutheAccountWebApi, AuthAccountWebApi>();
            services.AddScoped<IOperationalAccountWebApi, OperationalAccountWebApi>();
            return services;
        }

        #endregion
    }
}
