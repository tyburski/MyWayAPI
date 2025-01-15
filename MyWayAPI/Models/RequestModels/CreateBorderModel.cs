namespace MyWayAPI.Models.Body
{
    public class CreateBorderModel : RouteEventModel
    {
        public string BorderFrom { get; set; }
        public string BorderTo { get; set; }
    }
}
