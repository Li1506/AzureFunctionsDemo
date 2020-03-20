using System.Collections.Generic;
using Newtonsoft.Json;

namespace Demo.Functions.EntityFrameworkCore
{
    public partial class User
    {
        public User()
        {
            RentArrangementClients = new HashSet<RentArrangement>();
            RentArrangementManagers = new HashSet<RentArrangement>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        [JsonIgnore]
        public virtual ICollection<RentArrangement> RentArrangementClients { get; set; }
        [JsonIgnore]
        public virtual ICollection<RentArrangement> RentArrangementManagers { get; set; }
    }
}
