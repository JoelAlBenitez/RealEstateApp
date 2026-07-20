using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Contracts.PropertyType;
using RealEstateApp.Core.Application.Services.PropertyType;
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
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Dashboard;
using RealEstateApp.Core.Application.Contracts.Admin;
using RealEstateApp.Core.Application.Services.Admin;
using RealEstateApp.Core.Application.Contracts.Dashboard;
using RealEstateApp.Core.Application.Services.Dashboard;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.PropertyType;
using RealEstateApp.Core.Application.Mapping.EntityToDtoAndReverse.PropertyType;

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

                #region maper customer
                configuration.AddMaps(typeof(PropertyMappingProfile).Assembly);
                configuration.AddMaps(typeof(PropertyDtoToViewModelAndReverse).Assembly);
                configuration.AddMaps(typeof(FavoritePropertyMappingProfile).Assembly);
                configuration.AddMaps(typeof(FavoritePropertyDtoToViewModelAndReverse).Assembly);
                configuration.AddMaps(typeof(OfferMappingProfile).Assembly);
                configuration.AddMaps(typeof(OfferDtoToViewModelAndReverse).Assembly);
                configuration.AddMaps(typeof(MessageMappingProfile).Assembly);
                configuration.AddMaps(typeof(MessageDtoToViewModelAndReverse).Assembly);
                #endregion

                #region maper admin
                configuration.AddMaps(typeof(DashboardDtoToViewModel).Assembly);
                configuration.AddMaps(typeof(GetInternalUserDtoToAdministratorViewModel).Assembly);
                configuration.AddMaps(typeof(GetInternalUserDtoToDeveloperViewModel).Assembly);
                #endregion

                #region maper PropertyType
                configuration.AddProfile<PropertyTypeDtoToViewModelAndReverse>();
                configuration.AddProfile<PropertyTypeEntityToDtoAndReverse>();
                #endregion
            });

            #region Property Type Services
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<IPropertyTypeValidationService, PropertyTypeValidationService>();
            #endregion

            #region customer services
            services.AddScoped<IPropertyQueryService, PropertyQueryService>();
            services.AddScoped<IAgentPropertyService, AgentPropertyService>();
            services.AddScoped<IPropertyCascadeService, PropertyCascadeService>();
            services.AddScoped<IPropertyApiQueryService, PropertyApiQueryService>();
            services.AddScoped<IFavoritePropertyService, FavoritePropertyService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IMessageAtCService, MessageAtCService>();

            services.AddScoped<IPropertyValidationService, PropertyValidationService>();
            services.AddScoped<IFavoritePropertyValidationService, FavoritePropertyValidationService>();
            services.AddScoped<IOfferValidationService, OfferValidationService>();
            services.AddScoped<IMessageAtCValidationService, MessageAtCValidationService>();
            #endregion


            #region admin services
            services.AddScoped<IAdministratorService, AdministratorService>();
            services.AddScoped<IAdministratorValidationService, AdministratorValidationService>();
            services.AddScoped<IDeveloperService, DeveloperService>();
            services.AddScoped<IDashboardService, DashboardService>();
            #endregion
            return services;
        }
    }
}
