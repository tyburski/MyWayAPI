using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models.App;

namespace MyWayAPI.TypesConfigurations
{
    public class AppUserConfiguration: IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Companies)
                .WithMany(u => u.AppUsers);
            builder.HasMany(u => u.Invitations)
                .WithOne(u => u.AppUser)
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
