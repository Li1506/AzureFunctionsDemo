using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Functions.EntityFrameworkCore;
using Demo.Functions.Extensions;
using Demo.Functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demo.Functions
{
    public class PaymentInArrears
    {
        private const string Route = "paymentarrears";
        private readonly RentalArrearsContext _rentalArrearContext;

        public PaymentInArrears(RentalArrearsContext rentalArrearContext)
        {
            _rentalArrearContext = rentalArrearContext;
        }

        [FunctionName("PaymentInArrears")]
        public async Task<IActionResult> GetPaymentInArrears(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)]HttpRequest req,
            ILogger log)
        {
            var localNow = DateTime.UtcNow.ToAEST();

            var payments = await _rentalArrearContext.RentPayments
              .Include(x => x.RentArrangement.Property.Location)
              .Include(x => x.RentArrangement.Client)
              .Include(x => x.WeeklyCalendar)
              .Where(x => x.PaidDate == null && x.WeeklyCalendar.EndDate < localNow)
              .ToListAsync();

            var results = payments.Select(Converter.ConvertToPaymentInArrears).ToList();
            return new OkObjectResult(results);
        }
    }
}
