using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Infraestructure.Identity.Entities;

namespace RealEstateApp.Infraestructure.Identity.Context
{
    public class DbContextIdentityRealStateApp : IdentityDbContext<AppUsers>
    {

        public DbContextIdentityRealStateApp(DbContextOptions<DbContextIdentityRealStateApp> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Identity");
            builder.Entity<AppUsers>().ToTable("Users");
            builder.Entity<IdentityRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UsersLogin");

            builder.Entity<AppUsers>().Property(u => u.Name).HasMaxLength(50).IsRequired();
            builder.Entity<AppUsers>().Property(u => u.LastName).HasMaxLength(50).IsRequired();

            builder.Entity<AppUsers>().Property(u => u.IDCard).HasMaxLength(11);
            builder.Entity<AppUsers>().HasIndex(u => u.IDCard).IsUnique();

            builder.Entity<AppUsers>().Property(u => u.IsActive).IsRequired();

            builder.Entity<AppUsers>().Property(u => u.ProfileImg).HasMaxLength(int.MaxValue);

        }
    }
}
