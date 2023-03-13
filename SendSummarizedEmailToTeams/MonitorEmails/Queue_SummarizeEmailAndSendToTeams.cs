using System;
using Azure.Identity;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace MonitorEmails
{
    public class Queue_SummarizeEmailAndSendToTeams
    {
        [FunctionName("Queue_SummarizeEmailAndSendToTeams")]
        public void Run([QueueTrigger("emails-to-summarize", Connection = "queuetrigger")] EmailToSummarize myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var azureCredential = new DefaultAzureCredential();

            var graphClient = new GraphServiceClient(azureCredential);

            var delta = graphClient.Me.MailFolders.GetAsync();
        }
    }
}
