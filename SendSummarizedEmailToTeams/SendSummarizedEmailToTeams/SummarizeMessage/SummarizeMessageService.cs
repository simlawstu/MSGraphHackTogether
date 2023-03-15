using Azure.AI.TextAnalytics;

using SendSummarizedEmailToTeams.Abstractions;

namespace SendSummarizedEmailToTeams.SummarizeMessage
{
    public class SummarizeMessageService : ISummarizeMessageService
    {
        private readonly IFactory<TextAnalyticsClient> _clientFactory;

        public SummarizeMessageService(IFactory<TextAnalyticsClient> clientFactory)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<SummarizedMessage> SummarizeMessage(MessageToSummarize messageToSummarize)
        {
            var batchInput = new List<string>()
            {
                messageToSummarize.Subject,
                messageToSummarize.Body
            };

            var actions = new TextAnalyticsActions()
            {
                ExtractSummaryActions = new List<ExtractSummaryAction>() { new ExtractSummaryAction() }
            };

            var client = _clientFactory.Build();
            var operation = await client.StartAnalyzeActionsAsync(batchInput, actions);
            await operation.WaitForCompletionAsync();

            var listOfSentences = new List<string>();

            await foreach (var documentsInPage in operation.GetValuesAsync())
            {
                var summaryResults = documentsInPage.ExtractSummaryResults;

                foreach (var summaryResult in summaryResults)
                {
                    foreach (var documentResult in summaryResult.DocumentsResults)
                    {
                        foreach (var sentence in documentResult.Sentences)
                        {
                            listOfSentences.Add(sentence.Text);
                        }
                    }
                }
            }

            var summarizedMessage = new SummarizedMessage(listOfSentences.ToArray());
            return summarizedMessage;
        }
    }
}
