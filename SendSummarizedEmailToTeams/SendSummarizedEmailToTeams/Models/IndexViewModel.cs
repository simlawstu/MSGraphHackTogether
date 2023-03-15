using SendSummarizedEmailToTeams.ChannelRetrieval;
using SendSummarizedEmailToTeams.MailRetrieval;

namespace SendSummarizedEmailToTeams.Models
{
    public class IndexViewModel
    {
        public bool EmailSelected => SelectedMail != null;

        public IEnumerable<RetrievedMail> Mail { get; set; } = Enumerable.Empty<RetrievedMail>();

        public RetrievedMail? SelectedMail { get; set; }

        public IEnumerable<RetrievedTeam> Teams { get; set; } = Enumerable.Empty<RetrievedTeam>();
    }
}
