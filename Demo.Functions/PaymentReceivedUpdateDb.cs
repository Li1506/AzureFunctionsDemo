using System.Threading.Tasks;
using Demo.Functions.EntityFrameworkCore;
using Demo.Functions.Extensions;
using Demo.Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demo.Functions
{
    public class PaymentReceivedUpdateDb
    {
        private readonly RentalArrearsContext _rentalArrearContext;

        public PaymentReceivedUpdateDb(RentalArrearsContext rentalArrearContext)
        {
            _rentalArrearContext = rentalArrearContext;
        }

        [FunctionName("PaymentReceivedUpdateDb")]
        public async Task Run([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var paymentData = context.GetInput<PaymentReceived>();
            log.LogInformation($"Payment received db update started: {paymentData}");

            var payment = await _rentalArrearContext.RentPayments
                .FirstOrDefaultAsync(x => x.ReferenceId == paymentData.PaymentId);
            if (payment != null)
            {
                payment.Paid = paymentData.Amount.ToDecimal();
                payment.PaidDate = paymentData.PaymentDate;
                await _rentalArrearContext.SaveChangesAsync();
            }

            log.LogInformation($"Payment received db update completed");
        }
    }
}
