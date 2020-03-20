using System.Collections.Generic;
using Newtonsoft.Json;

namespace Demo.Functions.EntityFrameworkCore
{
    public partial class RentalProperty
    {
        public RentalProperty()
        {
            RentArrangements = new HashSet<RentArrangement>();
        }

        public int Id { get; set; }
        public int LocationId { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Carports { get; set; }

        public virtual Location Location { get; set; }

        [JsonIgnore]
        public virtual ICollection<RentArrangement> RentArrangements { get; set; }
    }
}
