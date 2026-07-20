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
                ProfileImg = "Img/Users/c7893d58-5079-4d20-991b-c5761804b25f/c7893d58-5079-4d20-991b-c5761804b25f.png",
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
