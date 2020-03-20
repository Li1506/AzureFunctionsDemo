using System;
using System.Collections.Generic;

namespace Demo.Functions.EntityFrameworkCore
{
    public partial class RentArrangement
    {
        public RentArrangement()
        {
            RentPayments = new HashSet<RentPayment>();
        }

        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PropertyId { get; set; }
        public int ManagerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal WeeklyRent { get; set; }
        public Guid ReferenceId { get; set; }

        public virtual User Client { get; set; }
        public virtual User Manager { get; set; }
        public virtual RentalProperty Property { get; set; }

        public virtual ICollection<RentPayment> RentPayments { get; set; }
    }
}
