using AutoMapper;
using Microsoft.Graph;

namespace SendSummarizedEmailToTeams.MailRetrieval
{
    public class MailRetrievalService : IMailRetrievalService
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
            var mailboxes = await _client.Me.MailFolders.Request().GetAsync();
            var inbox = mailboxes.FirstOrDefault(mb => mb.DisplayName == "Inbox");
            var mailResponse = await _client.Me.MailFolders[inbox.Id].Messages.Request().GetAsync();
            var retrievedMail = _mapper.Map<IEnumerable<RetrievedMail>>(mailResponse);

            return retrievedMail;
        }
    }
}
