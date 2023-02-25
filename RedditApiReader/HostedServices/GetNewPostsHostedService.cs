using Azure.Core;
using Microsoft.EntityFrameworkCore;
using RedditApiReader.Data;
using RedditApiReader.Models;
using System.Net.Http.Headers;
using System.Text;

namespace RedditApiReader.HostedServices
{
    public class GetNewPostsHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<GetNewPostsHostedService> _logger;
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer? _timer = null;

        private string apiClientId;
        private string apiClientSecret;
        public static string OAUTH_API_DOMAIN = "https://oauth.reddit.com";
        public static string APP_ONLY_OAUTH = "https://www.reddit.com/api/v1/access_token";
        private static HttpClient client = new HttpClient();

        public GetNewPostsHostedService(ILogger<GetNewPostsHostedService> logger, IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _config = config;
            _scopeFactory = scopeFactory;

            client.DefaultRequestHeaders.Add("User-Agent", "NVH API App");
            apiClientId = _config["RedditApiWebApp:ClientId"];
            apiClientSecret = _config["RedditApiWebApp:ClientSecret"];
            SetAuth();
        }

        private async void SetAuth()
        {
            string url = APP_ONLY_OAUTH + "?grant_type=client_credentials";
            var req = new HttpRequestMessage(HttpMethod.Post, url);

            var authenticationString = $"{apiClientId}:{apiClientSecret}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.UTF8.GetBytes(authenticationString));
            req.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

            var task = client.SendAsync(req);
            var response = task.Result;
            if (response != null)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(jsonString);
                var returnedAccessCode = jsonObject.SelectToken("access_token");
                string accessToken = returnedAccessCode != null ? returnedAccessCode.ToString() : string.Empty;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(2));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            List<RedditInfoItem> returnedList = new List<RedditInfoItem>();
            string url = OAUTH_API_DOMAIN + "/new";

            var task = client.GetAsync(url);
            var response = task.Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogInformation(
                    "Bad request returned {0}!", response.StatusCode);
            }
            else if (response != null)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(jsonString);
                returnedList.AddRange(RedditInfoItem.GetRedditInfoItems(jsonObject));

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<RedditApiReaderContext>();
                    dbContext.AddRange(returnedList);
                    await dbContext.SaveChangesAsync();
                }

                _logger.LogInformation(
                    "Database successfully updated. Count: {Count}", count);
            }
            else
            {

                _logger.LogInformation("Nothing returned!");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
