using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;
using MyWayAPI.Models.App;

namespace MyWayAPI
{
    public class DataInjector
    {
        public static void Inject(IApplicationBuilder app)
        {
            using(var dbContext = new MWDbContext())
            {
                var admin = dbContext.Admin.AsNoTracking().Any();
                if(!admin)
                {
                    var newAdmin = new Admin();
                    newAdmin.Credentials("admin", "admin");
                    dbContext.Admin.Add(newAdmin);
                    dbContext.SaveChanges();
                }
                var appUsers = dbContext.AppUsers.AsNoTracking().Any();
                if (!appUsers)
                {
                    var newUser = new AppUser
                    {
                        FirstName = "Dawid",
                        LastName = "Tyburski"
                    };
                    newUser.SetEmailAddress("dtyburski0@gmail.com");
                    newUser.SetPassword("1234");
                    
                    var newVehicle = new Vehicle { AppUser = newUser, LicensePlate = "KR001X", Type = "BUS" };
                    dbContext.AppUsers.Add(newUser);
                    dbContext.Vehicles.Add(newVehicle);
                    dbContext.SaveChanges();
                }
            }
            
        }
    }
}
