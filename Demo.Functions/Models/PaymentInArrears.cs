using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Functions.Models
{
    public class PaymentInArrears
    {
        public Guid PaymentId { get; set; }
        public string Property { get; set; }
        public string Amount { get; set; }
        public string DueDate { get; set; }
        public string Client { get; set; }
        public string ClientEmail { get; set; }
        public string ClientMobile { get; set; }
    }
}
