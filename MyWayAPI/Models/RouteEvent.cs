namespace MyWayAPI.Models
{
    public class RouteEvent
    {
        public int Id { get; set; }
        public string Place {  get; set; }
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Route Route { get; set; }
    }
}
