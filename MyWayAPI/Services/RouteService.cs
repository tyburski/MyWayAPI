using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;
using MyWayAPI.Models.Body;
using MyWayAPI.Models.RequestModels;

namespace MyWayAPI.Services
{
    public interface IRouteService
    {
        public bool StartRoute(int? userId, StartModel body);
        public bool NewBorder(CreateBorderModel body);
        public bool NewPickup(CreatePickupModel body);
        public bool NewRefuel(CreateRefuelModel body);
        public bool Drop(DropModel body);
        public Models.Route? GetStartedRoute(int? userId);
        public bool FinishRoute(int routeId, int? userId);
    }
    public class RouteService : IRouteService
    {
        private readonly MWDbContext dbContext;
        public RouteService(MWDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool StartRoute(int? userId, StartModel body)
        {
            var user = dbContext.Users.Include(u => u.Routes).FirstOrDefault(u => u.Id == userId);
            if (user is null) return false;

            var company = dbContext.Companies.FirstOrDefault(c => c.Id == body.CompanyId);
            if (company is null) return false;

            var vehicle = dbContext.Vehicles.FirstOrDefault(v => v.Id == body.VehicleId);
            if (vehicle is null) return false;

            var startedRoute = user.Routes.Where(s => s.Finished == false).Any();

            if (startedRoute) return false;
            else
            {
                var route = new Models.Route() { User = user, Company = company, Vehicle = vehicle, Finished = false };
                var startEvent = new RouteEvent()
                {
                    EventName = "start",
                    Date = DateTime.Now,
                    Latitude = body.Latitude,
                    Longitude = body.Longitude,
                    Route = route
                };
                dbContext.RouteEvents.Add(startEvent);
                dbContext.Routes.Add(route);
                dbContext.SaveChanges();
                return true;
            }
        }
        public bool NewBorder(CreateBorderModel body)
        {
            var route = dbContext.Routes.FirstOrDefault(r => r.Id == body.RouteId);
            if (route is not null)
            {
                var newEvent = new RouteEvent
                {
                    EventName = "border",
                    Date = new DateTime(),
                    Latitude = body.Latitude,
                    Longitude = body.Longitude,
                    PickupCount = null,
                    PickupWeight = null,
                    PickupComment = null,
                    DropDate = null,
                    DropLatitude = null,
                    DropLongitude = null,
                    RefuelCount = null,
                    RefuelTotal = null,
                    RefuelCurrency = null,
                    RefuelType = null,
                    BorderFrom = body.BorderFrom,
                    BorderTo = body.BorderTo,
                    Route = route
                };
                route.CurrentCountry = body.BorderTo;
                dbContext.Routes.Update(route);
                dbContext.RouteEvents.Add(newEvent);
                dbContext.SaveChanges();
                return true;
            }
            else return false;
        }
        public bool NewPickup(CreatePickupModel body)
        {
            var route = dbContext.Routes.FirstOrDefault(r => r.Id == body.RouteId);
            if (route != null)
            {
                var newEvent = new RouteEvent
                {
                    EventName = "pickup",
                    Date = new DateTime(),
                    Latitude = body.Latitude,
                    Longitude = body.Longitude,
                    PickupCount = body.PickupCount,
                    PickupWeight = body.PickupWeight,
                    PickupComment = body.PickupComment,
                    DropDate = null,
                    DropLatitude = null,
                    DropLongitude = null,
                    RefuelCount = null,
                    RefuelTotal = null,
                    RefuelCurrency = null,
                    RefuelType = null,
                    BorderFrom = null,
                    BorderTo = null,
                    Route = route
                };
                dbContext.RouteEvents.Add(newEvent);
                dbContext.SaveChanges();
                return true;
            }
            else return false;
        }
        public bool NewRefuel(CreateRefuelModel body)
        {
            var route = dbContext.Routes.FirstOrDefault(r => r.Id == body.RouteId);
            if (route != null)
            {
                var newEvent = new RouteEvent
                {
                    EventName = "refuel",
                    Date = new DateTime(),
                    Latitude = body.Latitude,
                    Longitude = body.Longitude,
                    PickupCount = null,
                    PickupWeight = null,
                    PickupComment = null,
                    DropDate = null,
                    DropLatitude = null,
                    DropLongitude = null,
                    RefuelCount = body.RefuelCount,
                    RefuelTotal = body.RefuelTotal,
                    RefuelCurrency = body.RefuelCurrency,
                    RefuelType = body.RefuelType,
                    BorderFrom = null,
                    BorderTo = null,
                    Route = route
                };
                dbContext.RouteEvents.Add(newEvent);
                dbContext.SaveChanges();
                return true;
            }
            else return false;
        }
        public bool Drop(DropModel body)
        {
            var routeEvent = dbContext.RouteEvents.FirstOrDefault(r => r.Id == body.EventId);
            if (routeEvent is not null && routeEvent.PickupWeight is not null && routeEvent.PickupCount is not null)
            {
                routeEvent.DropDate = new DateTime();
                routeEvent.DropLatitude = body.DropLatitude;
                routeEvent.DropLongitude = body.DropLongitude;
                dbContext.RouteEvents.Update(routeEvent);
                dbContext.SaveChanges();
                return true;
            }
            else return false;
        }
        public Models.Route? GetStartedRoute(int? userId)
        {
            var user = dbContext.Users.Include(u => u.Routes).ThenInclude(r => r.RouteEvents).FirstOrDefault(u => u.Id == userId);
            if (user is null) return null;

            var route = user.Routes.FirstOrDefault(r => r.Finished == false);
            if (route is null) return null;

            if (route is not null)
            {
                return route;
            }
            return null;
        }
        public bool FinishRoute(int routeId, int? userId)
        {
            var route = dbContext.Routes.FirstOrDefault(r => r.Id == routeId);
            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (route is null || route.User != user) return false;
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
