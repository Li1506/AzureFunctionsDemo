using System;

namespace Demo.Functions.EntityFrameworkCore
{
    public partial class WeeklyCalendar
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
