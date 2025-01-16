using Microsoft.EntityFrameworkCore;
using MyWayAPI.Models;

namespace MyWayAPI.Services
{
    public interface IVehicleService
    {
        public bool CreateVehicle(int? userId, string licensePlate, string type);
        public bool DeleteVehicle(int? userId, int vehicleId);
        public List<Vehicle> GetVehicles(int? userId);
    }
    public class VehicleService : IVehicleService
    {
        private readonly MWDbContext dbContext;
        public VehicleService(MWDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public bool CreateVehicle(int? userId, string licensePlate, string type)
        {
            var user = dbContext.Users.Include(u => u.Vehicles).FirstOrDefault(u => u.Id == userId);
            if (user is null) return false;

            var checkVehicle = user.Vehicles.FirstOrDefault(v => v.LicensePlate.Equals(licensePlate));
            if (checkVehicle is not null) return false;

            var newVeh = new Vehicle() { User = user, LicensePlate = licensePlate.ToUpper(), Type = type.ToUpper() };
            dbContext.Vehicles.Add(newVeh);
            dbContext.SaveChanges();
            return true;
        }

        public bool DeleteVehicle(int? userId, int vehicleId)
        {
            var user = dbContext.Users.Include(u => u.Vehicles).FirstOrDefault(u => u.Id == userId);
            if (user is null) return false;

            var vehicle = user.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle is null) return false;

            vehicle.User = null;

            dbContext.Vehicles.Update(vehicle);
            dbContext.SaveChanges();
            return true;
        }

        public List<Vehicle> GetVehicles(int? userId)
        {
            var vehicles = dbContext.Vehicles.Where(v => v.UserId == userId).ToList();
            if (vehicles is null) return [];
            else return vehicles;
        }
    }
}
