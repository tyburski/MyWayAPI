namespace MyWayAPI.Models.RequestModels
{
    public class FinishModel
    {
        public int RouteId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Country { get; set; }
    }
}
