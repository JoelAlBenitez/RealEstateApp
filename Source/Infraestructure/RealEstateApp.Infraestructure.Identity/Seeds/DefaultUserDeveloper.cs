
using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;

namespace RealEstateApp.Infraestructure.Identity.Seeds
{
    public static class DefaultUserDeveloper
    {
        public static async Task AddDefaultUserDeveloper(UserManager<AppUsers> userManager)
        {
            AppUsers users = new AppUsers
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sebastian",
                LastName = "Peguero",
                EmailConfirmed = true,
                Email = "sebastiandev@gmail.com",
                UserName = "PegueroS",
                IDCard = "00911222245",
                ProfileImg = "NA",
                IsActive = true,
                CreateAt = DateTimeOffset.UtcNow
            };

            if (!userManager.Users.Any(u => u.Id == users.Id))
            {
                var en = await userManager.FindByEmailAsync(users.Email);
                if (en == null)
                {
                    await userManager.CreateAsync(users, "passWord1234!");
                    await userManager.AddToRoleAsync(users, Roles.Desarrollador.ToString());
                }
            }
        }
    }
}
