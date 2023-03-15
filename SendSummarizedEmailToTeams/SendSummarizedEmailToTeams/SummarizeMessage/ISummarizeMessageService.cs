using Azure.AI.TextAnalytics;
using SendSummarizedEmailToTeams.MailRetrieval;

namespace SendSummarizedEmailToTeams.SummarizeMessage
{
    public interface ISummarizeMessageService
    {
        Task<SummarizedMessage> SummarizeMessage(MessageToSummarize messageToSummarize);
    }
}
