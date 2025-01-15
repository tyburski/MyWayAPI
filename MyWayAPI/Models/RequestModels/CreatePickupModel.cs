namespace MyWayAPI.Models.Body
{
    public class CreatePickupModel : RouteEventModel
    {
        public float PickupCount { get; set; }
        public float PickupWeight { get; set; }
        public string PickupComment { get; set; }
    }
}
