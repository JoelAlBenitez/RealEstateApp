using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infraestructure.Persistence.Configurations
{
    public class FavoritePropertyConfiguration : IEntityTypeConfiguration<FavoriteProperty>
    {
        public void Configure(EntityTypeBuilder<FavoriteProperty> builder)
        {
            builder.ToTable("FavoriteProperties");

            builder.HasKey(fp => fp.Id);

            builder.Property(fp => fp.CustomerId)
                .IsRequired();

            builder.HasIndex(fp => new { fp.CustomerId, fp.PropertyId })
                .IsUnique();

            builder.HasOne(fp => fp.Property)
                .WithMany()
                .HasForeignKey(fp => fp.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
