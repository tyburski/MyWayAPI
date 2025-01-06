using MyWayAPI.Models.Web;

namespace MyWayAPI.Models
{
    public class CreateCompanyModel
    {
        public string Name { get; set; }
        public int TaxId { get; set; }
        public string OwnerEmailAddress { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
    }
}
