using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySite.Entities.Concrete;

namespace MySite.Data.Concrete.EntityFramework.Mappings
{
    public class RoleClaimMap : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.HasKey(rc => rc.Id);
            builder.ToTable("RoleClaims");
        }
    }
}