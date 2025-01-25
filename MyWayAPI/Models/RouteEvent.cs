namespace MyWayAPI.Models
{
    public class RouteEvent
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }

        public float? RefuelCount { get; set; }
        public float? RefuelTotal { get; set; }
        public string? RefuelCurrency { get; set; }
        public string? RefuelType { get; set; }


        public float? PickupCount { get; set; }
        public float? PickupWeight { get; set; }
        public string? PickupComment { get; set; }
        public DateTime? DropDate { get; set; }
        public float? DropLatitude { get; set; }
        public float? DropLongitude { get; set; }

        public string? BorderFrom { get; set; }
        public string? BorderTo { get; set; }
    }
}
