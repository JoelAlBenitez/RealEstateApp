using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Properties;
using RealEstateApp.Infraestructure.Persistence.Repositories.Offers;
using RealEstateApp.Infraestructure.Persistence.Repositories.FavoriteProperties;
using RealEstateApp.Infraestructure.Persistence.Repositories.Messages;
using RealEstateApp.Infraestructure.Persistence.Repositories.PropertyImprovements;
using RealEstateApp.Infraestructure.Persistence.Repositories.PropertyTypeRepo;

namespace RealStateApp.IOC
{
    public static class InfraestructurePersistenceDependencies
    {
        public static IServiceCollection AddInfraestructurePersistence(
            this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<DbContextRealEstateApp>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<DbContextRealEstateApp>(options =>
                options.UseInMemoryDatabase("RealEstateAppDb"));   // -> pruebas uso de memory

            #region customer repositories
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IFavoritePropertyRepository, FavoritePropertyRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            #endregion
            services.AddScoped<IPropertyImprovementRepository, PropertyImprovementRepository>();

            #region Property Type Repositories
            services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
            #endregion

            return services;
        }
    }
}
