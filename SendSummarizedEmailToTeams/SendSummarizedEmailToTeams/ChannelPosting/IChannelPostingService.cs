using Microsoft.Graph;

namespace SendSummarizedEmailToTeams.ChannelPosting
{
    public interface IChannelPostingService
    {
        Task<ChatMessage?> PostMessageToChannel(string teamId, string channelId, MessageToPost requestBody);
    }
}
