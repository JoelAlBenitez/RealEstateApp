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
                ProfileImg = "Img/Users/667d9056-dee5-4c6a-8723-13eed6440374/667d9056-dee5-4c6a-8723-13eed6440374.jpg",
                IDCard = null,
                EmailConfirmed = true,
                PhoneNumber = "8290000001",
                Email = "adrian@gmail.com",
                CreateAt = DateTimeOffset.UtcNow
            };

            if(!userManager.Users.Any(u => u.Id == users.Id))
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
