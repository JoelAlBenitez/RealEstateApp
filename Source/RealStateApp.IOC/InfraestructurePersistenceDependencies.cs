using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Repositories.PropertyTypeRepo;

namespace RealStateApp.IOC
{
    public static class InfraestructurePersistenceDependencies
    {
        public static IServiceCollection AddInfraestructurePersistence(
            this IServiceCollection services, IConfiguration configuration)
        {

            ///

            #region Property Type Repositories
            services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
            #endregion

            return services;
        }
    }
}
