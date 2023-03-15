using AutoMapper;
using Microsoft.Graph;

namespace SendSummarizedEmailToTeams.ChannelRetrieval
{
    public class MapperProfile : Profile
    {
        public MapperProfile() {
            CreateMap<Team, RetrievedTeam>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName));
            CreateMap<Channel, RetrievedChannel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName));
        }
    }
}
