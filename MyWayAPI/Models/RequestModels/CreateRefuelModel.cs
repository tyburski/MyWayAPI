namespace MyWayAPI.Models.Body
{
    public class CreateRefuelModel : RouteEventModel
    {
        public float RefuelCount { get; set; }
        public float RefuelTotal { get; set; }
        public string RefuelCurrency { get; set; }
        public string RefuelType { get; set; }
    }
}
