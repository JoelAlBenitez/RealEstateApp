using Microsoft.EntityFrameworkCore;

namespace RealEstateApp.Infraestructure.Persistence.Context
{
    public class DbContextRealEstateApp: DbContext
    {
        public DbContextRealEstateApp(DbContextOptions<DbContextRealEstateApp> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
