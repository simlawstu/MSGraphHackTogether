using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using SendSummarizedEmailToTeams.ChannelRetrieval;
using SendSummarizedEmailToTeams.ChannelPosting;
using SendSummarizedEmailToTeams.MailRetrieval;
using SendSummarizedEmailToTeams.SummarizeMessage;
using Azure.AI.TextAnalytics;
using SendSummarizedEmailToTeams.Abstractions;

var builder = WebApplication.CreateBuilder(args);

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();



builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

var services = builder.Services;
services.Configure<CognitiveServicesOptions>(builder.Configuration.GetSection(CognitiveServicesOptions.ConfigKey));
services.AddScoped<IMailRetrievalService, MailRetrievalService>();
services.AddScoped<IChannelPostingService, ChannelPostingService>();
services.AddScoped<IChannelRetrievalService, ChannelRetrievalService>();
services.AddScoped<ISummarizeMessageService, SummarizeMessageService>();
services.AddScoped<IFactory<TextAnalyticsClient>, TextAnalyticsClientFactory>();
services.AddAutoMapper((config) =>
    {
        config.AddProfile<SendSummarizedEmailToTeams.MailRetrieval.MapperProfile>();
        config.AddProfile<SendSummarizedEmailToTeams.ChannelRetrieval.MapperProfile>();
        config.AddProfile<SendSummarizedEmailToTeams.Controllers.MapperProfile>();
    });

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllers();

app.Run();
