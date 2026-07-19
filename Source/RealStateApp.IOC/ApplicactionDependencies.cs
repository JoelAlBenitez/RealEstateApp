using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Auth;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Consult;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Operational;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.FavoriteProperties;
using RealEstateApp.Core.Application.Contracts.Offers;
using RealEstateApp.Core.Application.Contracts.Messages;
using RealEstateApp.Core.Application.Services.Properties;
using RealEstateApp.Core.Application.Services.FavoriteProperties;
using RealEstateApp.Core.Application.Services.Offers;
using RealEstateApp.Core.Application.Services.MessagesAtC;
using RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.Properties;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Property;
using RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.FavoriteProperty;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.FavoriteProperty;
using RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.Offer;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Offer;
using RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.Message;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Message;

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
                configuration.AddMaps(typeof(ResendEmailConfirmDtoToViewModelAndReverse).Assembly);
                #endregion

                #region propertys public
                configuration.AddMaps(typeof(PropertyFilterDtoToViewModel).Assembly);
                configuration.AddMaps(typeof(PropertyPublicDtoToViewModel).Assembly);
                configuration.AddMaps(typeof(PropertyDetailPublicConsultDtoToViewModel).Assembly);
                configuration.AddMaps(typeof(ConsultAgentByNameOrLastNameDtoToViewModel).Assembly);
                #endregion

                #region maper customer
                configuration.AddProfile<PropertyMappingProfile>();
                configuration.AddProfile<PropertyDtoToViewModelAndReverse>();
                configuration.AddProfile<FavoritePropertyMappingProfile>();
                configuration.AddProfile<FavoritePropertyDtoToViewModelAndReverse>();
                configuration.AddProfile<OfferMappingProfile>();
                configuration.AddProfile<OfferDtoToViewModelAndReverse>();
                configuration.AddProfile<MessageMappingProfile>();
                configuration.AddProfile<MessageDtoToViewModelAndReverse>();
                #endregion
            });

            #region customer services
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IFavoritePropertyService, FavoritePropertyService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IMessageAtCService, MessageAtCService>();

            services.AddScoped<IPropertyValidationService, PropertyValidationService>();
            services.AddScoped<IFavoritePropertyValidationService, FavoritePropertyValidationService>();
            services.AddScoped<IOfferValidationService, OfferValidationService>();
            services.AddScoped<IMessageAtCValidationService, MessageAtCValidationService>();
            #endregion

            return services;
        }
    }
}

