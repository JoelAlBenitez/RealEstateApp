using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infraestructure.Persistence.Context;

namespace RealEstateApp.Presentation.WebApp.Helpers
{
    public static class PropertyTypeDataSeeds
    {
        public static async Task SeedPropertyTypesAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContextRealEstateApp>();

            if (await dbContext.Set<PropertyType>().AnyAsync())
                return;

            var types = new List<PropertyType>
            {
                new PropertyType
                {
                    Name = "Casa",
                    Description = "Vivienda unifamiliar independiente o adosada con terreno propio.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new PropertyType
                {
                    Name = "Apartamento",
                    Description = "Unidad habitacional dentro de un edificio con áreas comunes compartidas.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new PropertyType
                {
                    Name = "Villa",
                    Description = "Residencia campestre o de playa con amplias áreas verdes y lujos.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new PropertyType
                {
                    Name = "Solar",
                    Description = "Terreno o parcela desocupada lista para construir edificaciones.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                }
            };

            await dbContext.Set<PropertyType>().AddRangeAsync(types);
            await dbContext.SaveChangesAsync();
        }
    }
}
