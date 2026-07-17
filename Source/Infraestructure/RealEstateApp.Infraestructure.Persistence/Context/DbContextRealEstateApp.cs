using Microsoft.EntityFrameworkCore;

using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infraestructure.Persistence.Context
{
    public class DbContextRealEstateApp: DbContext
    {
        public DbContextRealEstateApp(DbContextOptions<DbContextRealEstateApp> options)
            : base(options) { }

        public DbSet<PropertyType> PropertyTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
