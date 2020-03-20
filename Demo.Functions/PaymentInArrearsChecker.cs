using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Functions.EntityFrameworkCore;
using Demo.Functions.Extensions;
using Demo.Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demo.Functions
{
    public class PaymentInArrearsChecker
    {
        private readonly RentalArrearsContext _rentalArrearContext;

        public PaymentInArrearsChecker(RentalArrearsContext rentalArrearContext)
        {
            _rentalArrearContext = rentalArrearContext;
        }

        [FunctionName("PaymentInArrearsChecker")]
        public async Task Run([TimerTrigger("%PaymentInArrearsSchedule%")]TimerInfo myTimer,
            ILogger log,
             [Queue("arrearsqueue", Connection = "AzureWebJobsStorage")] ICollector<ArrearQueueMessage> queueCollector)
        {

            log.LogInformation($"Payments in arrears checker started");

            var localNow = DateTime.UtcNow.ToAEST();
            var paymentArrears = await _rentalArrearContext.RentPayments
                .Include(x => x.RentArrangement.Property.Location)
                .Include(x => x.RentArrangement.Client)
                .Include(x => x.WeeklyCalendar)
                .Where(x => x.PaidDate == null && x.WeeklyCalendar.EndDate < localNow)
                .ToListAsync();

            var messages = paymentArrears.GroupBy(x => x.RentArrangement.ManagerId)
                .Select(g =>
                {
                    var rent = g.FirstOrDefault()?.RentArrangement;
                    if (rent == null) return null;

                    return new ArrearQueueMessage
                    {
                        Manager = rent.Manager.Name,
                        ManagerEmail = rent.Manager.Email,
                        ManagerMobile = rent.Manager.Mobile,
                        PaymentInArrears = g.Select(Converter.ConvertToPaymentInArrears).ToList(),
                    };
                });
            foreach (var message in messages)
            {
                queueCollector.Add(message);
            }

            log.LogInformation($"Payments in arrears checker completed");
        }
    }
}
