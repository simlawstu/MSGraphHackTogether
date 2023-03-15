using AutoMapper;

using Microsoft.Graph;

namespace SendSummarizedEmailToTeams.ChannelPosting
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<MessageToPost, ChatMessage>()
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => new ItemBody() { Content = src.Body }));
        }
    }
}
