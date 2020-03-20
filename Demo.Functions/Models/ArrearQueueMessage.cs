using System.Collections.Generic;

namespace Demo.Functions.Models
{
    public class ArrearQueueMessage
    {
        public string Manager { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerMobile { get; set; }
        public IList<PaymentInArrears> PaymentInArrears { get; set; }
    }
}
