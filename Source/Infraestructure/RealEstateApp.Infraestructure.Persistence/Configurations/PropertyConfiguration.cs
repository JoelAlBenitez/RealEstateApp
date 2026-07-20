using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infraestructure.Persistence.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToTable("Properties");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(p => p.Code)
                .IsUnique();

            builder.HasIndex(p => new { p.Status, p.CreateAt });

            builder.HasIndex(p => p.AgentId);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.Size)
                .IsRequired();

            builder.Property(p => p.Bedrooms)
                .IsRequired();

            builder.Property(p => p.Bathrooms)
                .IsRequired();

            builder.Property(p => p.AgentId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.PropertyTypeId)
                .IsRequired();

            builder.Property(p => p.SaleTypeId)
                .IsRequired();

            builder.HasMany(p => p.Images)
                .WithOne(i => i.Property)
                .HasForeignKey(i => i.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.PropertyType)
                .WithMany()
                .HasForeignKey(p => p.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.SaleType)
                .WithMany()
                .HasForeignKey(p => p.SaleTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
