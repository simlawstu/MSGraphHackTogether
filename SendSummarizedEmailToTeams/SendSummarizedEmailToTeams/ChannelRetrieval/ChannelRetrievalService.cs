using AutoMapper;
using Microsoft.Graph;

namespace SendSummarizedEmailToTeams.ChannelRetrieval
{
    public class ChannelRetrievalService : IChannelRetrievalService
    {
        private readonly GraphServiceClient _client;
        private readonly IMapper _mapper;

        public ChannelRetrievalService(GraphServiceClient client, IMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RetrievedTeam>> GetTeamChannels()
        {
            var teams = await _client.Me.JoinedTeams.Request().GetAsync();

            var teamsRetrieved = _mapper.Map<IEnumerable<RetrievedTeam>>(teams);
            foreach (var team in teamsRetrieved)
            {
                var channels = await _client.Teams[team.Id].Channels.Request().GetAsync();
                team.Channels = _mapper.Map<IEnumerable<RetrievedChannel>>(channels);
            }

            return teamsRetrieved;
        }
    }
}
