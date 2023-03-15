using AutoMapper;
using Microsoft.Graph;

namespace SendSummarizedEmailToTeams.MailRetrieval
{
    public class MailRetrievalService
    {
        private readonly GraphServiceClient _client;
        private readonly IMapper _mapper;

        public MailRetrievalService(GraphServiceClient client, IMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RetrievedMail>> GetMail()
        {
            var mailResponse = await _client.Me.MailFolders["Index"].Messages.GetAsync();
            var retrievedMail = _mapper.Map<IEnumerable<RetrievedMail>>(mailResponse);

            return retrievedMail;
        }
    }
}
