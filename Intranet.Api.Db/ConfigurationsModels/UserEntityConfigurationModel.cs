using Intranet.Api.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intranet.Api.Db.ConfigurationsModels
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
