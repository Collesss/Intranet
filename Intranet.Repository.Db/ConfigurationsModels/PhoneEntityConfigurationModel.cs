using Intranet.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intranet.Repository.Db.ConfigurationsModels
{
    public class PhoneEntityConfigurationModel : IEntityTypeConfiguration<PhoneEntity>
    {
        public void Configure(EntityTypeBuilder<PhoneEntity> builder)
        {
            builder.HasKey(phone => phone.Id);

            builder
                .HasOne(phone => phone.User)
                .WithMany(user => user.Phones)
                .HasForeignKey(phone => phone.UserId)
                .HasPrincipalKey(user => user.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
