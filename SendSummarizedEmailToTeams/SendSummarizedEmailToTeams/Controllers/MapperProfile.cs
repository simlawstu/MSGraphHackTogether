using AutoMapper;

using SendSummarizedEmailToTeams.ChannelPosting;
using SendSummarizedEmailToTeams.MailRetrieval;
using SendSummarizedEmailToTeams.SummarizeMessage;

namespace SendSummarizedEmailToTeams.Controllers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RetrievedMail, MessageToSummarize>();
            CreateMap<SummarizedMessage, MessageToPost>();
        }
    }
}
