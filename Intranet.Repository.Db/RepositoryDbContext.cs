using Intranet.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Repository.Db
{
    public class RepositoryDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }


        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}
