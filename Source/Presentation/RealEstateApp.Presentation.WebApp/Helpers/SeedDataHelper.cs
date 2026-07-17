using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Domain.Common.Enums.PropertyStatus;
using RealEstateApp.Core.Domain.Common.Enums.OfferStatus;
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

            if (!context.Properties.Any())
            {
                var agent = await userManager.FindByEmailAsync("sebas@gmail.com");
                if (agent == null) return;

                var customer = await userManager.FindByEmailAsync("adrian@gmail.com");
                var now = DateTimeOffset.UtcNow;

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
                    PropertyTypeId = 2,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

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
                    PropertyTypeId = 3,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

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
                    PropertyTypeId = 1,
                    SaleTypeId = 2,
                    CreateAt = now,
                    UpdateAt = now
                };

                var p4 = new Property
                {
                    Code = "754129",
                    Price = 90000.00m,
                    Description = "Excelente solar urbanizado listo para construir. Cuenta con todas las conexiones de servicios básicos en una calle asfaltada de alta plusvalía.",
                    Size = 450.0,
                    Bedrooms = 0,
                    Bathrooms = 0,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 4,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

                var p5 = new Property
                {
                    Code = "632598",
                    Price = 185000.00m,
                    Description = "Moderno apartamento de 3 habitaciones en torre exclusiva. Terminación de primera, cocina modular, balcón tipo terraza y seguridad 24/7.",
                    Size = 145.0,
                    Bedrooms = 3,
                    Bathrooms = 2,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 2,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

                var p6 = new Property
                {
                    Code = "415286",
                    Price = 280000.00m,
                    Description = "Espaciosa casa en sector cerrado con portón eléctrico y parque infantil. 4 habitaciones amplias, cuarto de servicio y family room cómodo.",
                    Size = 310.0,
                    Bedrooms = 4,
                    Bathrooms = 3,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 1,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

                var p7 = new Property
                {
                    Code = "369852",
                    Price = 2500.00m,
                    Description = "Villa amueblada en alquiler vacacional de fin de semana. Piscina privada, amplio jardín arbolado, cocina equipada completa y zona de parrilla exterior.",
                    Size = 250.0,
                    Bedrooms = 3,
                    Bathrooms = 3,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 3,
                    SaleTypeId = 2,
                    CreateAt = now,
                    UpdateAt = now
                };

                var p8 = new Property
                {
                    Code = "258147",
                    Price = 115000.00m,
                    Description = "Apartamento acogedor de 1 habitación en zona universitaria. Ideal para inversión en plataformas de renta a corto plazo o para estudiantes.",
                    Size = 65.0,
                    Bedrooms = 1,
                    Bathrooms = 1,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 2,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

                var p9 = new Property
                {
                    Code = "159357",
                    Price = 320000.00m,
                    Description = "Hermosa propiedad residencial de diseño contemporáneo. Espacios abiertos, ventanales de piso a techo, patio con piscina y terraza social.",
                    Size = 340.0,
                    Bedrooms = 3,
                    Bathrooms = 3,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 1,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

                var p10 = new Property
                {
                    Code = "486259",
                    Price = 85000.00m,
                    Description = "Terreno amplio en zona campestre ideal para villa de retiro. Vista panorámica a las montañas y clima fresco durante todo el año.",
                    Size = 1200.0,
                    Bedrooms = 0,
                    Bathrooms = 0,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 4,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

                var p11 = new Property
                {
                    Code = "753951",
                    Price = 210000.00m,
                    Description = "Penthouse de dos niveles con excelente vista despejada a la ciudad. Jacuzzi privado en el segundo nivel, bar y área de barbacoa propia.",
                    Size = 210.0,
                    Bedrooms = 3,
                    Bathrooms = 3,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 2,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

                var p12 = new Property
                {
                    Code = "951753",
                    Price = 450000.00m,
                    Description = "Mansión señorial en una de las zonas más exclusivas. Terminación de roble, recibidor de doble altura, biblioteca y piscina con cascada.",
                    Size = 600.0,
                    Bedrooms = 5,
                    Bathrooms = 6,
                    AgentId = agent.Id,
                    Status = PropertyState.Available,
                    PropertyTypeId = 1,
                    SaleTypeId = 1,
                    CreateAt = now,
                    UpdateAt = now
                };

                await context.Properties.AddRangeAsync(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
                await context.SaveChangesAsync();

                var img1 = new PropertyImage { PropertyId = p1.Id, ImageUrl = "Img/Properties/seed1/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img2 = new PropertyImage { PropertyId = p2.Id, ImageUrl = "Img/Properties/seed2/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img3 = new PropertyImage { PropertyId = p3.Id, ImageUrl = "Img/Properties/seed3/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img4 = new PropertyImage { PropertyId = p4.Id, ImageUrl = "Img/Properties/seed1/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img5 = new PropertyImage { PropertyId = p5.Id, ImageUrl = "Img/Properties/seed2/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img6 = new PropertyImage { PropertyId = p6.Id, ImageUrl = "Img/Properties/seed3/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img7 = new PropertyImage { PropertyId = p7.Id, ImageUrl = "Img/Properties/seed1/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img8 = new PropertyImage { PropertyId = p8.Id, ImageUrl = "Img/Properties/seed2/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img9 = new PropertyImage { PropertyId = p9.Id, ImageUrl = "Img/Properties/seed3/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img10 = new PropertyImage { PropertyId = p10.Id, ImageUrl = "Img/Properties/seed1/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img11 = new PropertyImage { PropertyId = p11.Id, ImageUrl = "Img/Properties/seed2/photo1.jpg", CreateAt = now, UpdateAt = now };
                var img12 = new PropertyImage { PropertyId = p12.Id, ImageUrl = "Img/Properties/seed3/photo1.jpg", CreateAt = now, UpdateAt = now };

                await context.PropertyImages.AddRangeAsync(img1, img2, img3, img4, img5, img6, img7, img8, img9, img10, img11, img12);

                var imp1 = new PropertyImprovement { PropertyId = p1.Id, ImprovementId = 3 };
                var imp2 = new PropertyImprovement { PropertyId = p1.Id, ImprovementId = 6 };
                var imp3 = new PropertyImprovement { PropertyId = p2.Id, ImprovementId = 1 };
                var imp4 = new PropertyImprovement { PropertyId = p2.Id, ImprovementId = 2 };
                var imp5 = new PropertyImprovement { PropertyId = p3.Id, ImprovementId = 4 };
                var imp6 = new PropertyImprovement { PropertyId = p3.Id, ImprovementId = 5 };
                var imp7 = new PropertyImprovement { PropertyId = p5.Id, ImprovementId = 3 };
                var imp8 = new PropertyImprovement { PropertyId = p5.Id, ImprovementId = 6 };
                var imp9 = new PropertyImprovement { PropertyId = p6.Id, ImprovementId = 4 };
                var imp10 = new PropertyImprovement { PropertyId = p9.Id, ImprovementId = 1 };
                var imp11 = new PropertyImprovement { PropertyId = p11.Id, ImprovementId = 5 };
                var imp12 = new PropertyImprovement { PropertyId = p12.Id, ImprovementId = 1 };

                await context.PropertyImprovements.AddRangeAsync(imp1, imp2, imp3, imp4, imp5, imp6, imp7, imp8, imp9, imp10, imp11, imp12);
                await context.SaveChangesAsync();

                if (customer != null)
                {
                    var o1 = new Offer { CustomerId = customer.Id, PropertyId = p1.Id, Amount = 115000.00m, Status = OfferState.Pending, CreateAt = now, UpdateAt = now };
                    var o2 = new Offer { CustomerId = customer.Id, PropertyId = p2.Id, Amount = 340000.00m, Status = OfferState.Pending, CreateAt = now, UpdateAt = now };
                    var o3 = new Offer { CustomerId = customer.Id, PropertyId = p3.Id, Amount = 1400.00m, Status = OfferState.Pending, CreateAt = now, UpdateAt = now };
                    var o4 = new Offer { CustomerId = customer.Id, PropertyId = p5.Id, Amount = 180000.00m, Status = OfferState.Pending, CreateAt = now, UpdateAt = now };
                    var o5 = new Offer { CustomerId = customer.Id, PropertyId = p6.Id, Amount = 275000.00m, Status = OfferState.Pending, CreateAt = now, UpdateAt = now };
                    var o6 = new Offer { CustomerId = customer.Id, PropertyId = p8.Id, Amount = 110000.00m, Status = OfferState.Pending, CreateAt = now, UpdateAt = now };
                    var o7 = new Offer { CustomerId = customer.Id, PropertyId = p9.Id, Amount = 310000.00m, Status = OfferState.Pending, CreateAt = now, UpdateAt = now };
                    var o8 = new Offer { CustomerId = customer.Id, PropertyId = p11.Id, Amount = 200000.00m, Status = OfferState.Pending, CreateAt = now, UpdateAt = now };

                    await context.Offers.AddRangeAsync(o1, o2, o3, o4, o5, o6, o7, o8);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
