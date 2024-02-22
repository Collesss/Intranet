using Intranet.Repository.Db.ConfigurationsModels;
using Intranet.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Repository.Db
{
    public class RepositoryDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<PhoneEntity> Phones { get; set; }

        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserEntityConfigurationModel());
            modelBuilder.ApplyConfiguration(new PhoneEntityConfigurationModel());
        }
    }
}
