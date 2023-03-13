using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MonitorEmails
{
    public class Timer_GetProcessesToExecute
    {
        [FunctionName(nameof(Timer_GetProcessesToExecute))]
        public void Run(
            [TimerTrigger("*/5 * * * * *")]TimerInfo myTimer,
            //[Queue("emails-to-summarize", Connection = "queuetrigger")] ICollector<EmailToSummarize> collector,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Get from some Data Store the email account to summarize.


            // Trigger other functions to process the sumarrization.
            //collector.Add(new EmailToSummarize());
        }
    }
}
