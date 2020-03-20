using System;
using Newtonsoft.Json;

namespace Demo.Functions.EntityFrameworkCore
{
    public partial class RentPayment
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public int RentArrangementId { get; set; }
        public int WeeklyCalendarId { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DueDate { get; set; }
        public decimal? Paid { get; set; }
        public DateTime? PaidDate { get; set; }

        [JsonIgnore]
        public virtual RentArrangement RentArrangement { get; set; }
        public virtual WeeklyCalendar WeeklyCalendar { get; set; }
    }
}
