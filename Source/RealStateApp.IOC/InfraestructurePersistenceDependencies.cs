using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Domain.Interfaces.GenericRepository;
using RealEstateApp.Core.Domain.Interfaces.Repositories;
using RealEstateApp.Infraestructure.Persistence.Context;
using RealEstateApp.Infraestructure.Persistence.Repositories.Generic;
using RealEstateApp.Infraestructure.Persistence.Repositories.Properties;
using RealEstateApp.Infraestructure.Persistence.Repositories.Offers;
using RealEstateApp.Infraestructure.Persistence.Repositories.FavoriteProperties;
using RealEstateApp.Infraestructure.Persistence.Repositories.Messages;

namespace RealStateApp.IOC
{
    public static class InfraestructurePersistenceDependencies
    {
        public static IServiceCollection AddInfraestructurePersistence(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContextRealEstateApp>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(DbContextRealEstateApp).Assembly.FullName)));

            services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IOfferRepository, OfferRepository>();
            services.AddTransient<IFavoritePropertyRepository, FavoritePropertyRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();

            return services;
        }
    }
}
