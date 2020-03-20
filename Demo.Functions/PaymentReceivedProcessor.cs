using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Demo.Functions
{
    public static class PaymentReceivedProcessor
    {
        [FunctionName("PaymentReceivedProcessor")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            log.LogInformation($"Payment received run orchestrator started");

            var paymentData = context.GetInput<PaymentReceived>();

            log.LogInformation($"fanning out");
            var parallelActivities = new List<Task>();
            parallelActivities.Add(context.CallActivityAsync("PaymentReceivedUpdateDb", paymentData));
            parallelActivities.Add(context.CallActivityAsync("PaymentReceivedCallOtherApi", paymentData));

            await Task.WhenAll(parallelActivities);

            log.LogInformation($"Payment received run orchestrator complete");
        }
    }
}
