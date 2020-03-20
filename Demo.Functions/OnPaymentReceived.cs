using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using Demo.Functions.Models;
using Demo.Functions.Extensions;

namespace Demo.Functions
{
    public static class OnPaymentReceived
    {
        [FunctionName("OnPaymentReceived")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "onpaymentreceived")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            log.LogInformation($"Payment received on {DateTime.UtcNow.ToAEST()}");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<PaymentReceived>(requestBody);
            log.LogInformation($"Payment data: {requestBody}");

            var instanceId = await starter.StartNewAsync("PaymentReceivedProcessor", data);
            var status = await starter.GetStatusAsync(instanceId, showHistory: false);

            if (status.RuntimeStatus == OrchestrationRuntimeStatus.Failed)
            {
                log.LogError($"Processing payment failed");
            }
            return new OkObjectResult("Payment processed succesfully");
        }
    }
}
