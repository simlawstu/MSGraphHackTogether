using System;
using System.Threading.Tasks;

using Azure.Data.Tables;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace SendSummarizedEmailToTeams.Func
{
    public class GetItemsToProcess
    {
        [FunctionName("GetItemsToProcess")]
        public async Task Run(
            [TimerTrigger("*/30 * * * * *")] TimerInfo myTimer,
            [Queue("items-to-process", Connection = "StorageConnection")] IAsyncCollector<ItemToProcess> collector,
            [Table("ItemsToProcess", Connection = "StorageConnection")] TableClient tableClient,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var itemsToProcess = tableClient.QueryAsync<ItemToProcess>();

            await foreach (ItemToProcess itemToProcess in itemsToProcess)
            {
                await collector.AddAsync(itemToProcess);
            }
        }
    }
}
