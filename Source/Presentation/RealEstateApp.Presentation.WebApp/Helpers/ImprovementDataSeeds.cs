using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Domain.Entities;
using RealEstateApp.Infraestructure.Persistence.Context;

namespace RealEstateApp.Presentation.WebApp.Helpers
{
    /// <summary>
    /// Siembra datos de prueba de Mejoras en la base de datos en memoria.
    /// </summary>
    public static class ImprovementDataSeeds
    {
        public static async Task SeedImprovementsAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DbContextRealEstateApp>();

            // Solo sembrar si no hay datos
            if (await dbContext.Set<Improvement>().AnyAsync())
                return;

            var improvements = new List<Improvement>
            {
                new Improvement
                {
                    Name = "Piscina",
                    Description = "Área de piscina recreativa o deportiva privada.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new Improvement
                {
                    Name = "Gimnasio",
                    Description = "Espacio acondicionado con equipamiento para ejercicios físicos.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new Improvement
                {
                    Name = "Parqueo",
                    Description = "Estacionamiento privado reservado para vehículos.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new Improvement
                {
                    Name = "Jardín",
                    Description = "Zona exterior con césped, plantas y flores ornamentales.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new Improvement
                {
                    Name = "Terraza",
                    Description = "Espacio exterior techado o abierto ideal para recreación.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new Improvement
                {
                    Name = "Seguridad 24/7",
                    Description = "Vigilancia constante y control de acceso peatonal y vehicular.",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                }
            };

            await dbContext.Set<Improvement>().AddRangeAsync(improvements);
            await dbContext.SaveChangesAsync();
        }
    }
}
