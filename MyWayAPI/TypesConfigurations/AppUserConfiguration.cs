using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;

namespace MyWayAPI.TypesConfigurations
{
    public class AppUserConfiguration: IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Companies)
                .WithMany(u => u.AppUsers);
        }
    }
}
