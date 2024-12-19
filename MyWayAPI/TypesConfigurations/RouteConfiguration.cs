using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyWayAPI.TypesConfigurations
{
    public class RouteConfiguration: IEntityTypeConfiguration<Models.Route>
    {
        public void Configure(EntityTypeBuilder<Models.Route> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.RouteEvents)
                .WithOne(u => u.Route)
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
