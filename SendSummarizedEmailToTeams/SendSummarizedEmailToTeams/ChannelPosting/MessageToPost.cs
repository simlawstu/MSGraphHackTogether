using Microsoft.Graph;

namespace SendSummarizedEmailToTeams.ChannelPosting
{
    public class MessageToPost
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}