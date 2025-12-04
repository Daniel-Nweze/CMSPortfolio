using CMSPortfolio.Data;
using CMSPortfolio.Services;
using System.Net.Http.Headers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

builder.Services.AddTransient<ContactSubmissionRepository>();
builder.Services.AddTransient<ContactFormService>();

builder.Services.AddMemoryCache();

// GitHub HttpClient
builder.Services.AddHttpClient<GitHubApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.github.com/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("CMSPortfolio/1.0 (+https://github.com/Daniel-Nweze/CMSPortfolio)");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Secondary (quote) HttpClient
builder.Services.AddHttpClient<SecondaryApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.quotable.io/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// BackgroundService som uppdaterar cache
builder.Services.AddHostedService<ApiCacheRefreshService>();

builder.Services.AddHostedService<ApiCacheRefreshService>();



WebApplication app = builder.Build();

await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
