using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Infraestructure.Identity.Seeds
{
    public static class DefaultRolesSystem
    {
        public static async Task AddDefaultRolesSystem(RoleManager<IdentityRole> userRole)
        {
            string[] defaultRoles =
            [
                Roles.Agente.ToString(),
                Roles.Desarrollador.ToString(),
                Roles.Administrador.ToString(),
                Roles.Cliente.ToString()
            ];

            foreach (var role in defaultRoles)
            {
                if (!await userRole.RoleExistsAsync(role))
                    await userRole.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
