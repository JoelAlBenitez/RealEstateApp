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
                PhoneNumber = "829-000-0000",
                Name = "Sebastian",
                LastName = "Peguero",
                ProfileImg = "Img/Users/3c9a74c8-8e4f-4d19-9d8e-5f6b2a17e3af/3c9a74c8-8e4f-4d19-9d8e-5f6b2a17e3af.jpg",
                UserName = "SPeguero",
                EmailConfirmed = true,
                IsActive = true,
                IDCard = "NA"
            };

            if(userManager.Users.Any(u => u.Id == user.Id))
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
