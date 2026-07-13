using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RealStateApp.IOC
{
    public static class InfraestructurePersistenceDependencies
    {
        public static IServiceCollection AddInfraestructurePersistence(
            this IServiceCollection services, IConfiguration configuration)
        {

            ///

            return services;
        }
    }
}
