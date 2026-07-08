using Microsoft.AspNetCore.Identity;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Infraestructure.Identity.Seeds
{
    public static class DefaultUserCustomer
    {
        public static async Task AddDefaultUserCustomer(UserManager<AppUsers> userManager)
        {
            AppUsers users = new AppUsers
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Adrian",
                LastName = "Brito",
                UserName = "ABrito",
                IsActive = true,
                ProfileImg = "Img/Users/bbdcf1d9-c6e0-438d-a508-a69fb0d6b718/bbdcf1d9-c6e0-438d-a508-a69fb0d6b718.jpg",
                IDCard = "NA",
                EmailConfirmed = true,
                PhoneNumber = "829-000-000",
                Email = "adrian@gmail.com"
            };

            if(userManager.Users.Any(u => u.Id == users.Id))
            {
                var en = await userManager.FindByEmailAsync(users.Email);
                if(en == null)
                {
                    await userManager.CreateAsync(users, "adrianB1234!");
                    await userManager.AddToRoleAsync(users, Roles.Cliente.ToString());
                }
            }
        }
    }
}
