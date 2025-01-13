using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWayAPI.Models.Web;

namespace MyWayAPI.TypesConfigurations
{
    public class WebUserConfiguration : IEntityTypeConfiguration<WebUser>
    {
        public void Configure(EntityTypeBuilder<WebUser> builder)
        {
            builder.HasKey(u => u.Id);

        }
    }
}
