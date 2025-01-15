using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;

namespace MyWayAPI
{
    public class DataInjector
    {
        public static void Inject(IApplicationBuilder app)
        {
            using(var dbContext = new MWDbContext())
            {              
                var appUsers = dbContext.Users.AsNoTracking().Any();
                if (!appUsers)
                {
                    var newUser = new User
                    {
                        FirstName = "Dawid",
                        LastName = "Tyburski"
                    };
                    newUser.SetEmailAddress("dtyburski0@gmail.com");
                    newUser.SetPassword("1234");
                    
                    var newVehicle = new Vehicle { User = newUser, LicensePlate = "KR001X", Type = "BUS" };
                    dbContext.Users.Add(newUser);
                    dbContext.Vehicles.Add(newVehicle);
                    dbContext.SaveChanges();
                }
            }
            
        }
    }
}
