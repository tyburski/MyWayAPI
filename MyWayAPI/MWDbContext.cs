﻿using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;
using System.Reflection;

namespace MyWayAPI
{
    public class MWDbContext : DbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Role> Roles { get; set; }
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
