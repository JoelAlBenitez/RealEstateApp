using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RealStateApp.IOC
{
    public static class InfraestructureSharedDependencies
    {
        public static IServiceCollection AddInfraestructrueShared(
            this IServiceCollection services, IConfiguration configuration)
        {

            return services;
        }
    }
}
