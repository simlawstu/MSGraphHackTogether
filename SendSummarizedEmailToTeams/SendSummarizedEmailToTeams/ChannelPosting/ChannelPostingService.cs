﻿using AutoMapper;
using Microsoft.Graph;

namespace SendSummarizedEmailToTeams.ChannelPosting
{
    public class ChannelPostingService : IChannelPostingService
    {
        private readonly GraphServiceClient _client;
        private readonly IMapper _mapper;

        public ChannelPostingService(GraphServiceClient client, IMapper mapper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ChatMessage?> PostMessageToChannel(string teamId, string channelId, MessageToPost messageToPost)
        {
            var chatMessage = _mapper.Map<ChatMessage>(messageToPost);

            var result = await _client.Teams[$"{teamId}"]
                .Channels[$"{channelId}"].Messages.Request().AddAsync(chatMessage);

            return result;
        }
    }
}
