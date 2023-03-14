using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using SendSummarizedEmailToTeams.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SendSummarizedEmailToTeams.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            string teamId = "bf93dcbf-7e6b-4373-a567-ea967265d678";
            string channelId = "19:b28ae3dbbadf484b9877fc342bce5709@thread.tacv2";
            var scopes = new[] { "ChannelMessage.Send", "Group.ReadWrite.All" };

            var credential = new DefaultAzureCredential();
            //var graphClient = new GraphServiceClient(credential, scopes);
            var graphClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });

            var requestBody = new ChatMessage
            {
                Body = new ItemBody
                {
                    Content = "Hello world",
                },
            };

            var result = graphClient.Teams[$"{teamId}"]
                .Channels[$"{channelId}"].Messages;
                //.PostAsync(requestBody);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
