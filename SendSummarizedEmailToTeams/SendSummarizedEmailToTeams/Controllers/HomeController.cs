using AutoMapper;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using SendSummarizedEmailToTeams.ChannelRetrieval;
using SendSummarizedEmailToTeams.ChannelPosting;
using SendSummarizedEmailToTeams.MailRetrieval;
using SendSummarizedEmailToTeams.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SendSummarizedEmailToTeams.Controllers
{
    [AuthorizeForScopes(Scopes = new[] { "user.read", "mail.readwrite", "channel.readbasic.all", "channelmessage.readwrite", "channelmessage.send", "team.readbasic.all" })]

    //[AuthorizeForScopes(Scopes = new[] { "Channel.ReadBasic.All" ,"ChannelMessage.ReadWrite" ,"ChannelMessage.Send" ,"email" ,"Mail.ReadWrite" ,"offline_access" ,"openid" ,"profile" ,"Team.ReadBasic.All" ,"User.ReadWrite.All" ,"email", "user.read"})]
    public class HomeController : Controller
    {
        private readonly IMailRetrievalService _mailRetrievalService;
        private readonly IChannelRetrievalService _channelRetrievalService;
        private readonly IChannelPostingService _channelPostingService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IMailRetrievalService mailRetrievalService,
            IChannelRetrievalService channelRetrievalService,
            IChannelPostingService channelPostingService,
            ILogger<HomeController> logger)
        {
            _mailRetrievalService = mailRetrievalService ?? throw new ArgumentNullException(nameof(mailRetrievalService));
            _channelRetrievalService = channelRetrievalService ?? throw new ArgumentNullException(nameof(channelRetrievalService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _channelPostingService = channelPostingService ?? throw new ArgumentNullException(nameof(channelPostingService));
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
                var selectedMessage = messages.FirstOrDefault(m => m.Id == emailId);
                viewModel.SelectedMail = selectedMessage;
                var requestBody = new ChatMessage 
                { 
                    Body = new ItemBody 
                    { 
                        Content = selectedMessage?.Body 
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
