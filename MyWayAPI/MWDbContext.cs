using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;
using MyWayAPI.Models.App;
using MyWayAPI.Models.Web;
using System.Reflection;

namespace MyWayAPI
{
    public class MWDbContext : DbContext
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<RouteEvent> RouteEvents { get; set; }

        private string _connectionString =
             "Server=(localdb)\\local;Database=MWDb;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
