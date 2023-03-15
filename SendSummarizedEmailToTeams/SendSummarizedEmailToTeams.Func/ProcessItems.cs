using System;
using System.Threading.Tasks;

using Azure.Core;
using Azure.Identity;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
//using Microsoft.Graph.Models;

namespace SendSummarizedEmailToTeams.Func
{
    public class ProcessItems
    {
        [FunctionName("ProcessItems")]
        public async Task Run([QueueTrigger("items-to-process", Connection = "StorageConnection")] ItemToProcess itemtoProcess, ILogger log)
        {
            var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
            //var tenantId = "common";
            
            //var clientId = "dfcd8bb1-e653-446d-b4f0-d6da247c4c1b";
            //var clientSecret ="kVO8Q~W0gzrhNNe1ADIX.6vhovmGIUdidO1x1cn~";
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var credential = new ClientSecretCredential(itemtoProcess.TenantId, clientId, clientSecret);

            var graphServiceClient = new GraphServiceClient(credential, scopes);

            
            var mailFolders = await graphServiceClient.Users[itemtoProcess.UserId].MailFolders.GetAsync();

            var mailDelta = await graphServiceClient.Users[itemtoProcess.UserId].MailFolders["inbox"].Messages.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Filter = $"receivedDateTime lt 2022-03-14";
            });
            var requestBody = new ChatMessage
            {
                Body = new ItemBody
                {
                    Content = "Hello world"
                },
            };

            var result = graphServiceClient.Teams[itemtoProcess.TeamId].Channels[itemtoProcess.ChannelId].Messages.PostAsync(requestBody);
        }
    }
}
