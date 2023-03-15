using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using SendSummarizedEmailToTeams.ChannelRetrieval;
using SendSummarizedEmailToTeams.ChannelPosting;
using SendSummarizedEmailToTeams.MailRetrieval;
using SendSummarizedEmailToTeams.Models;
using System.Diagnostics;
using SendSummarizedEmailToTeams.SummarizeMessage;
using AutoMapper;

namespace SendSummarizedEmailToTeams.Controllers
{
    [AuthorizeForScopes(Scopes = new[] { "user.read", "mail.readwrite", "channel.readbasic.all", "channelmessage.readwrite", "channelmessage.send", "team.readbasic.all" })]

    //[AuthorizeForScopes(Scopes = new[] { "Channel.ReadBasic.All" ,"ChannelMessage.ReadWrite" ,"ChannelMessage.Send" ,"email" ,"Mail.ReadWrite" ,"offline_access" ,"openid" ,"profile" ,"Team.ReadBasic.All" ,"User.ReadWrite.All" ,"email", "user.read"})]
    public class HomeController : Controller
    {
        private readonly IMailRetrievalService _mailRetrievalService;
        private readonly IChannelRetrievalService _channelRetrievalService;
        private readonly IChannelPostingService _channelPostingService;
        private readonly ISummarizeMessageService _summarizeMessageService;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IMailRetrievalService mailRetrievalService,
            IChannelRetrievalService channelRetrievalService,
            IChannelPostingService channelPostingService,
            ISummarizeMessageService summarizeMessageService,
            IMapper mapper,
            ILogger<HomeController> logger)
        {
            _mailRetrievalService = mailRetrievalService ?? throw new ArgumentNullException(nameof(mailRetrievalService));
            _channelRetrievalService = channelRetrievalService ?? throw new ArgumentNullException(nameof(channelRetrievalService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _channelPostingService = channelPostingService ?? throw new ArgumentNullException(nameof(channelPostingService));
            _summarizeMessageService = summarizeMessageService ?? throw new ArgumentNullException(nameof(summarizeMessageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index(string emailId)
        {
            
            var messages = await _mailRetrievalService.GetMail();
            var teamChannels = await _channelRetrievalService.GetTeamChannels();
            
            var viewModel = new IndexViewModel();
            viewModel.Mail = messages;
            viewModel.Teams = teamChannels;

            if (!string.IsNullOrWhiteSpace(emailId))
            {
                var selectedMessage = messages.FirstOrDefault(m => m.Id == emailId);
                viewModel.SelectedMail = selectedMessage;
            }
            
            return View(viewModel);
        }

        public async Task<IActionResult> PostToChannel(string emailId, string teamId, string channelId)
        {
            var messages = await _mailRetrievalService.GetMail();
            var teamChannels = await _channelRetrievalService.GetTeamChannels();

            var viewModel = new IndexViewModel();
            viewModel.Mail = messages;
            viewModel.Teams = teamChannels;
            if (!string.IsNullOrWhiteSpace(emailId))
            {
                var selectedMessage = messages.First(m => m.Id == emailId);
                viewModel.SelectedMail = selectedMessage;
                var messagetoSummarize = _mapper.Map<MessageToSummarize>(selectedMessage);
                var summarizedmail = await _summarizeMessageService.SummarizeMessage(messagetoSummarize);

                var requestBody = new ChatMessage
                {
                    Subject = $"Summarized message: '{selectedMessage.Subject}' from: '{selectedMessage.From}'.",
                    Body = new ItemBody
                    {
                        Content = summarizedmail.Summary,
                    },
                };
                var result = await _channelPostingService.PostMessageToChannel(teamId, channelId, requestBody);
            }

            return View("index",viewModel);
        }

        public async Task<IActionResult> Privacy()
        {
            string teamId = "bf93dcbf-7e6b-4373-a567-ea967265d678";
            string channelId = "19:b28ae3dbbadf484b9877fc342bce5709@thread.tacv2";
                        
            var requestBody = new ChatMessage
            {
                Body = new ItemBody
                {
                    Content = "Hello world",
                },
            };

            var result = await _channelPostingService.PostMessageToChannel(teamId, channelId, requestBody);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
