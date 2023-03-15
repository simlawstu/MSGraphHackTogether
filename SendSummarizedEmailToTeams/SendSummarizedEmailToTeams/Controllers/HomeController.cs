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
            var viewModel = await GetViewModel(emailId);

            return View(viewModel);
        }

        public async Task<IActionResult> PostToChannel(string emailId, string teamId, string channelId)
        {
            if (string.IsNullOrEmpty(emailId))
            {
                throw new ArgumentException($"'{nameof(emailId)}' cannot be null or empty.", nameof(emailId));
            }

            if (string.IsNullOrEmpty(teamId))
            {
                throw new ArgumentException($"'{nameof(teamId)}' cannot be null or empty.", nameof(teamId));
            }

            if (string.IsNullOrEmpty(channelId))
            {
                throw new ArgumentException($"'{nameof(channelId)}' cannot be null or empty.", nameof(channelId));
            }

            var viewModel = await GetViewModel(emailId);

            var selectedMail = viewModel.SelectedMail;
            if(selectedMail == null)
            {
                throw new InvalidOperationException("Mail Selected doesn't actually exist");
            }

            var messagetoSummarize = _mapper.Map<MessageToSummarize>(viewModel.SelectedMail);
            var summarizedmail = await _summarizeMessageService.SummarizeMessage(messagetoSummarize);

            var requestBody = new MessageToPost
            {
                Subject = $"Summarized message: '{selectedMail.Subject}' from: '{selectedMail.From}'.",
                Body = summarizedmail.Summary
            };
            var result = await _channelPostingService.PostMessageToChannel(teamId, channelId, requestBody);

            return View("index",viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<IndexViewModel> GetViewModel(string emailId)
        {
            var messages = await _mailRetrievalService.GetMail();
            var teamChannels = await _channelRetrievalService.GetTeamChannels();

            var viewModel = new IndexViewModel
            {
                Mail = messages,
                Teams = teamChannels
            };

            if(!string.IsNullOrWhiteSpace(emailId))
            {
                var selectedMessage = messages.First(m => m.Id == emailId);
                viewModel.SelectedMail = selectedMessage;
            }

            return viewModel;
        }
    }
}
