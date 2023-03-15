using AutoMapper;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Web;
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
        private readonly IChannelPostingService _channelPostingService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IMailRetrievalService mailRetrievalService,
            ILogger<HomeController> logger,
            IChannelPostingService channelPostingService)
        {
            _mailRetrievalService = mailRetrievalService ?? throw new ArgumentNullException(nameof(mailRetrievalService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _channelPostingService = channelPostingService ?? throw new ArgumentNullException(nameof(channelPostingService));
        }

        public async Task<IActionResult >Index()
        {
            var messages = await _mailRetrievalService.GetMail();
            var viewModel = new IndexViewModel();
            viewModel.Mail = messages;

            return View(viewModel);
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
