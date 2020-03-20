using System.Collections.Generic;
using Newtonsoft.Json;

namespace Demo.Functions.EntityFrameworkCore
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
