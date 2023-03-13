using Azure.Identity;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace SummarizeEmailsAndSend
{
    public class Queue_SummarizeEmailAndSendToTeams
    {
        [FunctionName("Queue_SummarizeEmailAndSendToTeams")]
        public void Run([QueueTrigger("queueName", Connection = "queueConnection")] EmailToSummarize myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var azureCredential = new DefaultAzureCredential();

            //var graphClient = new GraphServiceClient(azureCredential);

            //var mailFolders = graphClient.Me.MailFolders.GetAsync();
        }
    }
}
