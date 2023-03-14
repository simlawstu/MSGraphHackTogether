using System;
using System.Threading.Tasks;

using Azure.Data.Tables;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace SendSummarizedEmailToTeams.Func
{
    public class GetItemsToProcess
    {
        [FunctionName("GetItemsToProcess")]
        public async Task Run(
            [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
             [Queue("ItemsToProcess", Connection = "StorageConnection")] IAsyncCollector<ItemToProcess> collector,

             //[Table("ItemsToProcess", Connection = "StorageConnection")] CloudTable tableClient,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            //var query = new TableQuery<ItemToProcess>();
            //var itemsToProcess = tableClient.);

            //await foreach (ItemToProcess itemToProcess in itemsToProcess)
            //{
            //    await collector.AddAsync(itemToProcess);
            //}
        }
    }
}
