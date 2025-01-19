using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyWayAPI.TypesConfigurations
{
    public class RouteConfiguration: IEntityTypeConfiguration<Models.Route>
    {
        public void Configure(EntityTypeBuilder<Models.Route> builder)
        {
            builder.HasKey(u => u.Id);


            builder.HasOne(u => u.Company)
                .WithMany(u => u.Routes)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(u => u.Vehicle)
                .WithMany(u => u.Routes)
                .HasForeignKey(u => u.VehicleId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasMany(u => u.RouteEvents)
                .WithOne(u => u.Route)
                .HasForeignKey(u => u.RouteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
