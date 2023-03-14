using System;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SendSummarizedEmailToTeams.Func
{
    public class ProcessItems
    {
        [FunctionName("ProcessItems")]
        public void Run([QueueTrigger("items-to-process", Connection = "StorageConnection")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
