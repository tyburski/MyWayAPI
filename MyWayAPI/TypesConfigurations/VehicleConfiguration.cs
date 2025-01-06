using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;

namespace MyWayAPI.TypesConfigurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Routes)
                .WithOne(u => u.Vehicle)
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
