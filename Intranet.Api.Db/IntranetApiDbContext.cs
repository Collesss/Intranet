using Intranet.Api.Db.ConfigurationsModels;
using Intranet.Api.Db.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Intranet.Api.Db
{
    public class IntranetApiDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<PhoneEntity> Phones { get; set; }

        public IntranetApiDbContext(DbContextOptions<IntranetApiDbContext> dbContextOptions) : base(dbContextOptions)
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

            //modelBuilder.Entity<UserEntity>().HasData(new UserEntity() { Id = 1, SID = new SecurityIdentifier(1).ToString(), DisplayName = "", Email = "", UserPrincipalName = ""});


            /*
            Enumerable.Range(1, 100)
                .Select(i =>
                {
                    byte[] sidBytes = new byte[SecurityIdentifier.MaxBinaryLength - 2];

                    random.NextBytes(sidBytes);
                    
                    sidBytes = sidBytes.Prepend((byte)5).Prepend((byte)1).ToArray();

                    return new UserEntity() { Id = i, SID = new SecurityIdentifier(sidBytes, 0).ToString(), DisplayName = $"Test{i} Test{1}", Email = $"test{i}@test.ru", UserPrincipalName = $"Test{i}" };
                })
                .ToList()
                .ForEach(user => Debug.WriteLine($"INSERT INTO [Intranet].[dbo].[Users] ([Id], [SID], [UserPrincipalName], [DisplayName], [Email]) VALUES({user.Id}, '{user.SID}', '{user.UserPrincipalName}', '{user.DisplayName}', '{user.Email}');"));

            Enumerable.Range(1, 100).SelectMany(i => new PhoneEntity[] {
                    new PhoneEntity() { Id = i*2-1, Type = "telephonenumber", PhoneNumbers = $"{i:D4}", UserId = i },
                    new PhoneEntity() { Id = i*2, Type = "ipphone", PhoneNumbers = $"{i:D4}", UserId = i } })
                .ToList()
                .ForEach(phone => Debug.WriteLine($"INSERT INTO [Intranet].[dbo].[Phones]([Id], [UserId], [Type] ,[PhoneNumbers]) VALUES ({phone.Id}, {phone.UserId}, '{phone.Type}', '{phone.PhoneNumbers}');"));
            */



            Random random = new Random();


            modelBuilder.Entity<UserEntity>().HasData(Enumerable.Range(1, 100)
                .Select(i =>
                {
                    byte[] sidBytes = new byte[SecurityIdentifier.MaxBinaryLength - 2];

                    random.NextBytes(sidBytes);

                    sidBytes = sidBytes.Prepend((byte)5).Prepend((byte)1).ToArray();

                    return new UserEntity() { Id = i, SID = new SecurityIdentifier(sidBytes, 0).ToString(), DisplayName = $"Test{i} Test{1}", Email = $"test{i}@test.ru", UserPrincipalName = $"Test{i}" };
                }));


            modelBuilder.Entity<PhoneEntity>().HasData(
                Enumerable.Range(1, 100).SelectMany(i => new PhoneEntity[] {
                    new PhoneEntity() { Id = i*2-1, Type = "telephonenumber", PhoneNumbers = $"{i:D4}", UserId = i },
                    new PhoneEntity() { Id = i*2, Type = "ipphone", PhoneNumbers = $"{i:D4}", UserId = i } }));

        }
    }
}
