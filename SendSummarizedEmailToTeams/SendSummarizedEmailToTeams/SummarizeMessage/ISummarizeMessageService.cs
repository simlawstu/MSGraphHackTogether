using Azure.AI.TextAnalytics;
using SendSummarizedEmailToTeams.MailRetrieval;

namespace SendSummarizedEmailToTeams.SummarizeMessage
{
    public interface ISummarizeMessageService
    {
        Task SummarizeMessage(RetrievedMail mail);

        Task TextSummarizationMessage(TextAnalyticsClient client, RetrievedMail mail);
    }
}
