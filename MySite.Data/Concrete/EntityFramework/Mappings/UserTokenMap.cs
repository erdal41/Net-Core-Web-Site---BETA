using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySite.Entities.Concrete;

namespace MySite.Data.Concrete.EntityFramework.Mappings
{
    public class UserTokenMap : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            builder.Property(t => t.LoginProvider).HasMaxLength(256);
            builder.Property(t => t.Name).HasMaxLength(256);
            builder.ToTable("UserTokens");
        }
    }
}