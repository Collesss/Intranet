using Intranet.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intranet.Repository.Db.ConfigurationsModels
{
    public class UserEntityConfigurationModel : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(user => user.Id);

            builder.HasAlternateKey(user => user.SID);
        }
    }
}
