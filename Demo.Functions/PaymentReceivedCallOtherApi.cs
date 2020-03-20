using System.Threading.Tasks;
using Demo.Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Demo.Functions
{
    public class PaymentReceivedCallOtherApi
    {
        [FunctionName("PaymentReceivedCallOtherApi")]
        public async Task Run([ActivityTrigger] IDurableActivityContext context, ILogger log)
        {
            var paymentData = context.GetInput<PaymentReceived>();
            log.LogInformation($"Payment received call other api started: {paymentData}");

            await Task.Delay(5000);

            log.LogInformation($"Payment received call other api completed");
        }
    }
}
