using System;

namespace Demo.Functions.Models
{
    public class PaymentReceived
    {
        public Guid PaymentId { get; set; }
        public string Amount { get; set; }
        public string Payer { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
