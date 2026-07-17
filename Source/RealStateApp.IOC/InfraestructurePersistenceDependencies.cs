using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Repositories.SaleTypeRepo;

namespace RealStateApp.IOC
{
    public static class InfraestructurePersistenceDependencies
    {
        public static IServiceCollection AddInfraestructurePersistence(
            this IServiceCollection services, IConfiguration configuration)
        {

            #region Sale Type Repositories
            services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();
            #endregion

            return services;
        }
    }
}
