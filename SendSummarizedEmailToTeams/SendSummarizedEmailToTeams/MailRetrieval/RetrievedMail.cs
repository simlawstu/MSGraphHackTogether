namespace SendSummarizedEmailToTeams.MailRetrieval
{
    public class RetrievedMail
    {
        public string? Id { get; set; }
        public string? From { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}