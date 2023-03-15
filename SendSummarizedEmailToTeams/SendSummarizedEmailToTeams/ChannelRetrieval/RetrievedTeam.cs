namespace SendSummarizedEmailToTeams.ChannelRetrieval
{
    public class RetrievedTeam
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public IEnumerable<RetrievedChannel> Channels { get; set; } = Enumerable.Empty<RetrievedChannel>();
    }
}