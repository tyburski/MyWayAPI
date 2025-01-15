namespace MyWayAPI.Models.RequestModels
{
    public class StartModel
    {
        public int VehicleId { get; set; }
        public int CompanyId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Country { get; set; }
    }
}
