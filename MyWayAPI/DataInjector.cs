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
                var appUsers = dbContext.AppUsers.Any();
                if (!appUsers)
                {
                    var newUser = new AppUser
                    {
                        FirstName = "Dawid",
                        LastName = "Tyburski"
                    };
                    newUser.SetEmailAddress("dtyburski0@gmail.com");
                    newUser.SetPassword("1234");
                    dbContext.AppUsers.Add(newUser);
                    dbContext.SaveChanges();
                }
            }
            
        }
    }
}
