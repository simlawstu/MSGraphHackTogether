using Azure;
using Azure.AI.TextAnalytics;

using Microsoft.Extensions.Options;

using SendSummarizedEmailToTeams.Abstractions;

namespace SendSummarizedEmailToTeams.SummarizeMessage
{
    public class TextAnalyticsClientFactory : IFactory<TextAnalyticsClient>
    {
        private CognitiveServicesOptions _options;

        public TextAnalyticsClientFactory(IOptions<CognitiveServicesOptions> options)
        {
            _options = options.Value;
        }

        public TextAnalyticsClient Build()
        {
            if (_options.Endpoint == null)
            {
                throw new InvalidOperationException("You must provide a value for endpoint.");
            }
            if (_options.Key == null)
            {
                throw new InvalidOperationException("You must provide a value for key.");
            }

            var endpointUri = new Uri(_options.Endpoint);
            var azureClientKey = new AzureKeyCredential(_options.Key);
            var client = new TextAnalyticsClient(endpointUri, azureClientKey);
            return client;
        }
    }
}
