using System.Collections.Generic;

namespace Demo.Functions.EntityFrameworkCore
{
    public partial class Location
    {
        public Location()
        {
            RentalProperty = new HashSet<RentalProperty>();
        }

        public int Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string State { get; set; }

        public virtual ICollection<RentalProperty> RentalProperty { get; set; }
    }
}
