using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infraestructure.Persistence.Context;

namespace RealEstateApp.Presentation.WebApp.Helpers
{
    /// <summary>
    /// Siembra datos de prueba de Tipos de Ventas en la base de datos en memoria.
    /// </summary>
    public static class SaleTypeDataSeeds
    {
        public static async Task SeedSaleTypesAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContextRealEstateApp>();

            // Solo sembrar si no hay datos
            if (await dbContext.Set<SaleType>().AnyAsync())
                return;

            var types = new List<SaleType>
            {
                new SaleType
                {
                    Name = "Venta",
                    Description = "Propiedades disponibles para adquisición de título definitivo mediante pago total.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new SaleType
                {
                    Name = "Alquiler",
                    Description = "Propiedades disponibles para arrendamiento mensual bajo contrato de alquiler.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                }
            };

            await dbContext.Set<SaleType>().AddRangeAsync(types);
            await dbContext.SaveChangesAsync();
        }
    }
}
