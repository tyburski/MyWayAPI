using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWayAPI.Models;

namespace MyWayAPI.TypesConfigurations
{
    public class WebUserConfiguration : IEntityTypeConfiguration<WebUser>
    {
        public void Configure(EntityTypeBuilder<WebUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Company)
                .WithMany(u => u.WebUsers)
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Role)
                .WithMany(u => u.WebUsers)
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
