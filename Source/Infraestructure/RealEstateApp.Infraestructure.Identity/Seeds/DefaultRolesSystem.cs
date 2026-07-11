using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Infraestructure.Identity.Seeds
{
    public static class DefaultRolesSystem
    {
        public static async Task AddDefaultRolesSystem(RoleManager<IdentityRole> userRole)
        {
            await userRole.CreateAsync(new IdentityRole(Roles.Agente.ToString()));
            await userRole.CreateAsync(new IdentityRole(Roles.Desarrollador.ToString()));
            await userRole.CreateAsync(new IdentityRole(Roles.Administrador.ToString()));
            await userRole.CreateAsync(new IdentityRole(Roles.Cliente.ToString()));
        }
    }
}
