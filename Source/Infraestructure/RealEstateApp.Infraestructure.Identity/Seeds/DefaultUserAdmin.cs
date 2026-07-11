
using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;

namespace RealEstateApp.Infraestructure.Identity.Seeds
{
    public static  class DefaultUserAdmin
    {
        public static async Task AddDefaultUserAdmin(UserManager<AppUsers> userManager)
        {
            AppUsers users = new AppUsers
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Joel",
                LastName = "Benitez",
                EmailConfirmed = true,
                Email = "joel@gmail.com",
                UserName = "JBenitez",
                IDCard = "00912333330",
                ProfileImg = "NA",
                IsActive = true,
                CreateAt = DateTimeOffset.UtcNow
            };

            if(!userManager.Users.Any(u => u.Id == users.Id))
            {
                var en = userManager.FindByEmailAsync(users.Email);
                if(en == null){
                    await userManager.CreateAsync(users, "passWord1234!");
                    await userManager.AddToRoleAsync(users, Roles.Administrador.ToString());
                }
            }
        }
    }
}
