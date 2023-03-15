using Azure;
using Azure.AI.TextAnalytics;

using Microsoft.Extensions.Options;

using SendSummarizedEmailToTeams.Abstractions;

namespace SendSummarizedEmailToTeams.SummarizeMessage
{
    public class TextAnalyticsClientFactory : IFactory<TextAnalyticsClient>
    {
        private readonly ILogger<TextAnalyticsClientFactory> _logger;
        private readonly CognitiveServicesOptions _options;

        public TextAnalyticsClientFactory(IOptions<CognitiveServicesOptions> options,
            ILogger<TextAnalyticsClientFactory> logger)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public TextAnalyticsClient Build()
        {
            _logger.LogInformation(_options.Endpoint);
            _logger.LogInformation(_options.Key);
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
