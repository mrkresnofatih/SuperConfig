using DesertCamel.BaseMicroservices.SuperConfig.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperConfig.EntityFramework
{
    public class SuperConfigDbContext : DbContext
    {
        public SuperConfigDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ConfigEntity> Configs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConfigEntity>()
                .HasIndex(p => p.Key)
                .IsUnique();
        }
    }
}
