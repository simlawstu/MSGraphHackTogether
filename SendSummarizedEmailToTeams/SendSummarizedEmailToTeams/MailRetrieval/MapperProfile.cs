using AutoMapper;
using Microsoft.Graph;

namespace SendSummarizedEmailToTeams.MailRetrieval
{
    public class MapperProfile : Profile
    {
        public MapperProfile() {
            CreateMap<Message, RetrievedMail>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From.EmailAddress.Name))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body.Content));
        }
    }
}
