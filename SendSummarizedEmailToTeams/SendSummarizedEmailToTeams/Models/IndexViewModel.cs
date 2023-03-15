using SendSummarizedEmailToTeams.MailRetrieval;

namespace SendSummarizedEmailToTeams.Models
{
    public class IndexViewModel
    {
        public IEnumerable<RetrievedMail> Mail { get; set; } = Enumerable.Empty<RetrievedMail>();

        public RetrievedMail? SelectedMail { get; set; }

    }
}
