using Azure;
using System;
using Azure.AI.TextAnalytics;
using System.Threading.Tasks;
using System.Collections.Generic;
using SendSummarizedEmailToTeams.MailRetrieval;

namespace SendSummarizedEmailToTeams.SummarizeMessage
{
    public class SummarizeMessageService : ISummarizeMessageService
    {
        private readonly AzureKeyCredential credentials = new AzureKeyCredential("key");
        private readonly Uri endpoint = new("https://localhost.com");

        public async Task SummarizeMessage(RetrievedMail mail)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            await TextSummarizationMessage(client, mail);
        }
        public async Task TextSummarizationMessage(TextAnalyticsClient client, RetrievedMail mail)
        {            
            string document = mail.ToString();
            var batchInput = new List<string>
            {
                document
            };

            TextAnalyticsActions actions = new TextAnalyticsActions()
            {
                ExtractKeyPhrasesActions = new List<ExtractKeyPhrasesAction>() { new ExtractKeyPhrasesAction() }
                //ExtractSummaryActions = new List<ExtractSummaryAction>() { new ExtractSummaryAction() }
            };

            //Start analysing process
            AnalyzeActionsOperation operation = await client.StartAnalyzeActionsAsync(batchInput, actions);
            await operation.WaitForCompletionAsync();

            //view operation result
            await foreach (AnalyzeActionsResult mailInPage in operation.GetValuesAsync()) //operation.values
            {
                IReadOnlyCollection<ExtractKeyPhrasesActionResult> summaryResults = mailInPage.ExtractKeyPhrasesResults;
                foreach (ExtractKeyPhrasesActionResult summaryActionResults in summaryResults)
                {
                    if (summaryActionResults.HasError)
                    {
                        continue;
                        //throw;
                    }

                    foreach (ExtractKeyPhrasesResult mailResults in summaryActionResults.DocumentsResults)
                    {
                        if (mailResults.HasError)
                        {
                            continue;
                        }

                        //foreach (SummarySentence sentence in mailResults.Sentences)
                        //{

                        //}
                    }
                }
            }
        }
    }
}
