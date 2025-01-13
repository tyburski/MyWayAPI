using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;
using MyWayAPI.Models.App;
using MyWayAPI.Models.Web;

namespace MyWayAPI.Services.App
{
    public interface IAppRouteService
    {
        public int StartRoute(int userId, int companyId, int vehicleId);
        public bool NewEvent(RouteEventBody body);
        public bool Drop(DropBody body);
        public List<RouteEvent> GetStartedRoute(int userId);
        public bool FinishRoute(int routeId, int userId);
    }
    public class AppRouteService: IAppRouteService
    {
        private readonly MWDbContext dbContext;
        public AppRouteService(MWDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int StartRoute(int userId, int companyId, int vehicleId)
        {
            var user = dbContext.AppUsers.Include(u=>u.Routes).FirstOrDefault(u => u.Id == userId);
            if (user is null) return 0;

            var company = dbContext.Companies.FirstOrDefault(c => c.Id == companyId);
            if(company is null) return 0;

            var vehicle = dbContext.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle is null) return 0;

            var startedRoute = user.Routes.Where(s => s.Finished == false).Any();

            if (startedRoute) return 0;
            else
            {
                var route = new Models.Route() { AppUser = user,Company=company,Vehicle=vehicle, Finished= false };
                dbContext.Routes.Add(route);
                dbContext.SaveChanges();
                return route.Id;

            }
        }
        public bool NewEvent(RouteEventBody body)
        {
            
            var route = dbContext.Routes.FirstOrDefault(r => r.Id == body.RouteId);
            if (route != null)
            {
                var newEvent = new RouteEvent
                {
                    EventName = body.EventName,
                    Date = body.Date,
                    Latitude = body.Latitude,
                    Longitude = body.Longitude,
                    Country = body.Country,
                    PickupCount = body.PickupCount,
                    PickupWeight = body.PickupWeight,
                    PickupComment = body.PickupComment,
                    DropDate = body.DropDate,
                    DropLatitude = body.DropLatitude,
                    DropLongitude = body.DropLongitude,
                    RefuelCount = body.RefuelCount,
                    RefuelTotal = body.RefuelTotal,
                    RefuelCurrency = body.RefuelCurrency,
                    RefuelType = body.RefuelType,
                    BorderFrom = body.BorderFrom,
                    BorderTo = body.BorderTo,
                    Route = route
                };
                dbContext.RouteEvents.Add(newEvent);
                dbContext.SaveChanges();
                return true;
            }
            else return false;           
        }
        public bool Drop(DropBody body)
        {

            var routeEvent = dbContext.RouteEvents.FirstOrDefault(r => r.Id == body.EventId);
            if (routeEvent != null)
            {
                routeEvent.DropDate = body.DropDate;
                routeEvent.DropLatitude = body.DropLatitude;
                routeEvent.DropLongitude = body.DropLongitude;
                dbContext.RouteEvents.Update(routeEvent);
                dbContext.SaveChanges();
                return true;
            }
            else return false;
        }



        public List<RouteEvent> GetStartedRoute(int userId)
        {
            var user = dbContext.AppUsers.Include(u=>u.Routes).ThenInclude(r=>r.RouteEvents).FirstOrDefault(u=>u.Id == userId);
            if(user is null) return new List<RouteEvent>();

            var route = user.Routes.FirstOrDefault(r => r.Finished == false);
            if(route is null) return new List<RouteEvent>();

            if(route.RouteEvents.Count!=null)
            {
                var routeEvents = route.RouteEvents.ToList();
                List<RouteEvent> SortedList = routeEvents.OrderByDescending(o => o.Date).ToList();
                return SortedList;
            }
            return new List<RouteEvent>();


        }
        public bool FinishRoute(int routeId, int userId)
        {
            var route = dbContext.Routes.FirstOrDefault(r=>r.Id == routeId);
            var user = dbContext.AppUsers.FirstOrDefault(u=>u.Id == userId);
            if(route is null|| route.AppUser != user) return false;
            else
            {
                route.Finished = true;
                dbContext.Routes.Update(route);
                dbContext.SaveChanges();
                return true;
            }
        }
    }
}
