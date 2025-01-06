using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;
using MyWayAPI.Models.App;
using MyWayAPI.Models.Web;

namespace MyWayAPI.Services.App
{
    public interface IAppRouteService
    {
        public bool StartRoute(int userId, int companyId, int vehicleId);
        public bool FinishRoute(int routeId, int userId);
    }
    public class AppRouteService: IAppRouteService
    {
        private readonly MWDbContext dbContext;
        public AppRouteService(MWDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool StartRoute(int userId, int companyId, int vehicleId)
        {
            var user = dbContext.AppUsers.FirstOrDefault(u => u.Id == userId);
            if (user is null) return false;

            var company = dbContext.Companies.FirstOrDefault(c => c.Id == companyId);
            if(company is null) return false;

            var vehicle = dbContext.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle is null) return false;

            if (user.startedRoute is not null) return false;
            else
            {
                var route = new Models.Route() { AppUser = user, Company = company, Vehicle = vehicle, Finished= false };
                dbContext.Routes.Add(route);
                user.startedRoute = route;
                dbContext.SaveChanges();
                return true;

            }
        }
        public bool FinishRoute(int routeId, int userId)
        {
            var route = dbContext.Routes.FirstOrDefault(r=>r.Id == routeId);
            var user = dbContext.AppUsers.FirstOrDefault(u=>u.Id == userId);
            if(route is null|| route.AppUser != user) return false;
            else
            {
                route.Finished = true;
                route.AppUser.startedRoute = null;
                dbContext.SaveChanges();
                return true;
            }
        }
    }
}
