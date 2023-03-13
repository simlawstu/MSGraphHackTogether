using Azure;
using Azure.Data.Tables;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SummarizeEmailsAndSend.TableStorageEntities;
using System;
using System.Threading.Tasks;

namespace SummarizeEmailsAndSend
{
    public class Timer_GetProcessesToExecute
    {
        [FunctionName(nameof(Timer_GetProcessesToExecute))]
        public static async Task Run([TimerTrigger("* */1 * * * *")] TimerInfo myTimer,
            [Queue("queueName", Connection = "queueConnection")] IAsyncCollector<EmailToSummarize> collector,
            [Table("emailsToProcess", "")] TableClient tableClient,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            AsyncPageable<EmailToProcess> all = tableClient.QueryAsync<EmailToProcess>();
            await foreach(EmailToProcess process in all)
            {
                await collector.AddAsync(new EmailToSummarize()
                {
                    UserId = process.UserId,
                    TeamId = process.TeamId
                });
            }
        }
    }
}
