namespace SendSummarizedEmailToTeams.ChannelRetrieval
{
    public interface IChannelRetrievalService
    {
        Task<IEnumerable<RetrievedTeam>> GetTeamChannels();
    }
}