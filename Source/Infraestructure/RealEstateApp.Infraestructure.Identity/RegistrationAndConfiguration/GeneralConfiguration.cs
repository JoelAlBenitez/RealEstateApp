using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Infraestructure.Identity.Context;

namespace RealEstateApp.Infraestructure.Identity.RegistrationAndConfiguration
{
    public static  class GeneralConfiguration
    {
        public static void AddGeneralConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContextIdentityRealStateApp>(opt =>
                opt.UseInMemoryDatabase("RealEstateAppIdentityDb"));
        }
    }
}
