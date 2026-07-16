using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Persistence.Context;

namespace RealEstateApp.Presentation.WebApp.Helpers
{
    public static class SeedDataHelper
    {
        public static async Task GenerateDataSeedProperties(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var userManager = services.GetRequiredService<UserManager<AppUsers>>();
            var context = services.GetRequiredService<DbContextRealEstateApp>();

            // Only seed if properties table is empty
            if (!context.Properties.Any())
            {
                var agent = await userManager.FindByEmailAsync("sebas@gmail.com");
                if (agent == null) return;

                var now = DateTimeOffset.UtcNow;

                // Property 1: Apartment
                var p1 = new Property
                {
                    Code = "832941",
                    Price = 120000.00m,
                    Description = "Hermoso apartamento moderno en el centro de la ciudad con excelente iluminación natural y acceso a áreas comunes. Ideal para parejas jóvenes o ejecutivos.",
                    Size = 95.0,
                    Bedrooms = 2,
                    Bathrooms = 2,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 2, // Apartamento
                    SaleTypeId = 1, // Venta
                    CreateAt = now,
                    UpdateAt = now
                };

                // Property 2: Villa
                var p2 = new Property
                {
                    Code = "921475",
                    Price = 350000.00m,
                    Description = "Espectacular villa de lujo frente al mar. Cuenta con amplias terrazas, acabados en mármol y un entorno residencial privado con máxima seguridad.",
                    Size = 380.0,
                    Bedrooms = 4,
                    Bathrooms = 5,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 3, // Villa
                    SaleTypeId = 1, // Venta
                    CreateAt = now,
                    UpdateAt = now
                };

                // Property 3: Casa
                var p3 = new Property
                {
                    Code = "143892",
                    Price = 1500.00m,
                    Description = "Cómoda casa familiar de dos niveles en zona residencial tranquila. Posee amplio patio delantero y terraza trasera perfecta para BBQ.",
                    Size = 220.0,
                    Bedrooms = 3,
                    Bathrooms = 2,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 1, // Casa
                    SaleTypeId = 2, // Alquiler
                    CreateAt = now,
                    UpdateAt = now
                };

                await context.Properties.AddRangeAsync(p1, p2, p3);
                await context.SaveChangesAsync();

                // Seed Images
                var img1 = new PropertyImage
                {
                    PropertyId = p1.Id,
                    ImageUrl = "Img/Properties/seed1/photo1.jpg",
                    CreateAt = now,
                    UpdateAt = now
                };
                var img2 = new PropertyImage
                {
                    PropertyId = p2.Id,
                    ImageUrl = "Img/Properties/seed2/photo1.jpg",
                    CreateAt = now,
                    UpdateAt = now
                };
                var img3 = new PropertyImage
                {
                    PropertyId = p3.Id,
                    ImageUrl = "Img/Properties/seed3/photo1.jpg",
                    CreateAt = now,
                    UpdateAt = now
                };

                await context.PropertyImages.AddRangeAsync(img1, img2, img3);

                // Seed Improvements
                var imp1 = new PropertyImprovement { PropertyId = p1.Id, ImprovementId = 3 }; // Parqueo
                var imp2 = new PropertyImprovement { PropertyId = p1.Id, ImprovementId = 6 }; // Seguridad 24/7
                var imp3 = new PropertyImprovement { PropertyId = p2.Id, ImprovementId = 1 }; // Piscina
                var imp4 = new PropertyImprovement { PropertyId = p2.Id, ImprovementId = 2 }; // Gimnasio
                var imp5 = new PropertyImprovement { PropertyId = p3.Id, ImprovementId = 4 }; // Jardín
                var imp6 = new PropertyImprovement { PropertyId = p3.Id, ImprovementId = 5 }; // Terraza

                await context.PropertyImprovements.AddRangeAsync(imp1, imp2, imp3, imp4, imp5, imp6);
                await context.SaveChangesAsync();
            }
        }
    }
}
