using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.Contracts.Messages;
using RealEstateApp.Core.Application.Services.Properties;
using RealEstateApp.Core.Application.Services.FavoriteProperties;
using RealEstateApp.Core.Application.Services.Offers;
using RealEstateApp.Core.Application.Services.MessagesAtC;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Auth;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Consult;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Operational;

namespace RealStateApp.IOC
{
    public static class ApplicactionDependencies
    {
        public static IServiceCollection AddApplicationDepdencies(
            this IServiceCollection services)
        {
            services.AddAutoMapper(configuration =>
            {
                #region maper users
                configuration.AddMaps(typeof(ForgoutPasswordDtoToViewModelAndReverse).Assembly);
                configuration.AddMaps(typeof(LoginUserDtoToViewModelAndReverse).Assembly);
                configuration.AddMaps(typeof(ResetPasswordDtoToViewModelAndReverse).Assembly);
                configuration.AddMaps(typeof(ConsultAgentDtoToViewModel).Assembly);
                configuration.AddMaps(typeof(CreateExternalUserDtoToViewModelAndReverse).Assembly);
                configuration.AddMaps(typeof(CreateInternalUserDtoToViewModelAndReverse).Assembly);
                configuration.AddMaps(typeof(EditExternalUserDtoToViewModelAndReverse).Assembly);
                configuration.AddMaps(typeof(EditInternalUserDtoToViewModelAndReverse).Assembly);
                #endregion 
            });

            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IFavoritePropertyService, FavoritePropertyService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IMessageAtCService, MessageAtCService>();

            services.AddTransient<IPropertyValidationService, PropertyValidationService>();
            services.AddTransient<IFavoritePropertyValidationService, FavoritePropertyValidationService>();
            services.AddTransient<IOfferValidationService, OfferValidationService>();
            services.AddTransient<IMessageAtCValidationService, MessageAtCValidationService>();

            return services;
        }
    }
}
