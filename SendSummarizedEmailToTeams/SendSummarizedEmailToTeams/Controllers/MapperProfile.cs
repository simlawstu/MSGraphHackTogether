using AutoMapper;

using SendSummarizedEmailToTeams.MailRetrieval;
using SendSummarizedEmailToTeams.SummarizeMessage;

namespace SendSummarizedEmailToTeams.Controllers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RetrievedMail, MessageToSummarize>();
        }
    }
}
