namespace SendSummarizedEmailToTeams.MailRetrieval
{
    public interface IMailRetrievalService
    {
        Task<IEnumerable<RetrievedMail>> GetMail();
    }
}