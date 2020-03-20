using System;
using System.Linq;
using System.Text;
using Demo.Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace Demo.Functions
{
    public static class PaymentInArrearsSendEmail
    {
        [FunctionName("PaymentInArrearsSendEmail")]
        public static async void Run([QueueTrigger("arrearsqueue", Connection = "AzureWebJobsStorage")]ArrearQueueMessage myQueueItem,
            ILogger log,
            IBinder binder)
        {
            log.LogInformation($"Payment in arrears send email started");

            var hasPayments = myQueueItem.PaymentInArrears != null && myQueueItem.PaymentInArrears.Any();
            if (!hasPayments)
            {
                log.LogInformation("No payment in arrears");
                return;
            }

            var enableEmail = bool.Parse(Environment.GetEnvironmentVariable("EnableEmail"));
            var testRecipient = Environment.GetEnvironmentVariable("TestRecipientEmail");
            var senderEmail = Environment.GetEnvironmentVariable("EmailSender");

            if (!enableEmail)
            {
                log.LogInformation("Email sending is not enabled");
                return;
            }

            var sendGridMessages = await binder.BindAsync<ICollector<SendGridMessage>>(new SendGridAttribute()
            {
                ApiKey = "SendGridApiKey"
            });

            var message = new SendGridMessage();
            message.From = new EmailAddress(senderEmail);
            message.Subject = "❗ Payment In Arrears Alert ❗";
            message.AddTo(string.IsNullOrEmpty(testRecipient) ? myQueueItem.ManagerEmail : testRecipient);

            var sb = new StringBuilder();
            sb.AppendLine($"<p>Dear {myQueueItem.Manager}:</p>");

            sb.AppendLine($"<p>The following payments in arrears require your attention.</p>");
            sb.AppendLine($"<table><tr><th>Property</th><th>Due Date</th><th>Amount Due</th><th>Client</th><th>Client Contact</th></th>");
            var paymentHtml = myQueueItem.PaymentInArrears?.Select(x =>
            {
                return $"<tr><td>{x.Property}</td><td>{x.DueDate}<td><td>{x.Amount}</td><td>{x.Client}</td><td>{x.ClientMobile}</td></tr>";
            }).Aggregate((i, j) => i + Environment.NewLine + j);
            sb.AppendLine(paymentHtml);
            sb.AppendLine("</table>");

            message.HtmlContent = sb.ToString();

            sendGridMessages.Add(message);

            log.LogInformation($"Payment in arrears send email complete");
        }
    }
}
