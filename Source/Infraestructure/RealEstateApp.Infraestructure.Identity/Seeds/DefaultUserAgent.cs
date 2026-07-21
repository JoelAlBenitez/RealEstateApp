using Microsoft.AspNetCore.Identity;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Infraestructure.Identity.Seeds
{
    public static class DefaultUserAgent
    {
        public static async Task AddDefaultUserAgent(UserManager<AppUsers> userManager)
        {
            AppUsers user = new AppUsers
            {
                Id = Guid.NewGuid().ToString(),
                Email = "sebas@gmail.com",
                PhoneNumber = "8290000000",
                Name = "Sebastian",
                LastName = "Peguero",
                ProfileImg = "Img/Users/667d9056-dee5-4c6a-8723-13eed6440374/667d9056-dee5-4c6a-8723-13eed6440374.jpg",
                UserName = "SPeguero",
                EmailConfirmed = true,
                IsActive = true,
                IDCard = null,
                CreateAt = DateTimeOffset.UtcNow

            };

            if(!userManager.Users.Any(u => u.Id == user.Id))
            {
                var en = await userManager.FindByEmailAsync(user.Email);
                if(en == null)
                {
                    await userManager.CreateAsync(user, "sebasTian1234!");
                    await userManager.AddToRoleAsync(user, Roles.Agente.ToString());
                }
            }
        }
    }
}
