using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infraestructure.Persistence.Context
{
    public class DbContextRealEstateApp: DbContext
    {
        public DbContextRealEstateApp(DbContextOptions<DbContextRealEstateApp> options)
            : base(options) { }

        public DbSet<SaleType> SaleTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SaleType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                      
                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasMaxLength(250);
            });
        }
    }
}
