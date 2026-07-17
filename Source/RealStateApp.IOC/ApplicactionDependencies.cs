using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Contracts.PropertyType;
using RealEstateApp.Core.Application.Services.PropertyType;
using RealEstateApp.Core.Application.Contracts.GenericServices;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Auth;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Consult;
using RealEstateApp.Core.Application.Mapping.DtoToViewModelAndReverse.Users.Operational;
using RealEstateApp.Core.Application.Services.Generic;

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

            #region Property Type Services
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<IPropertyTypeValidationService, PropertyTypeValidationService>();
            #endregion

            return services;
        }
    }
}
