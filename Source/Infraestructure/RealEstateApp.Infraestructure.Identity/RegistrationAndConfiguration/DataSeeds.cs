using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Seeds;

namespace RealEstateApp.Infraestructure.Identity.RegistrationAndConfiguration
{
    public static class DataSeeds
    {
        #region Data Seed User Base
        public static async Task GenerateDataSeedUsers(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.ServiceProvider;

            var users = service.GetRequiredService<UserManager<AppUsers>>();
            var roles = service.GetRequiredService<RoleManager<IdentityRole>>();
            await DefaultRolesSystem.AddDefaultRolesSystem(roles);
            await DefaultUserAdmin.AddDefaultUserAdmin(users);
            await DefaultUserAgent.AddDefaultUserAgent(users);
            await DefaultUserCustomer.AddDefaultUserCustomer(users);
            await DefaultUserDeveloper.AddDefaultUserDeveloper(users);

        }


        #endregion
    }
}
