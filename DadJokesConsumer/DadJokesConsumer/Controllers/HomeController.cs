using DadJokesConsumer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace DadJokesConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Settings _settings;
        private bool initializeJokesCount = false;
        private int jokesCount = 0;
        public HomeController(ILogger<HomeController> logger, Settings options)
        {
            _logger = logger;
            _settings = options;
        }

        public async Task<IActionResult> Index()
        {
            var apihost = _settings.APIHost;
            //DadJokeBody dadJokeBody = null;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_settings.DadJokesBaseURL+_settings.DadJokesRandomEndPoint),
                Headers =
                {
                    { "X-RapidAPI-Key", _settings.APIKey },
                    { "X-RapidAPI-Host", _settings.APIHost },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var dadJoke = JsonConvert.DeserializeObject<DadJoke>(body);
                ViewData.Model = dadJoke?.body?.FirstOrDefault();                
            }
            
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            if (initializeJokesCount)
            {
                ViewData.Model = jokesCount;
                return View();
            }
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_settings.DadJokesBaseURL+_settings.DadJokesCountEndPoint),
                Headers =
                {
                    { "X-RapidAPI-Key", _settings.APIKey },
                    { "X-RapidAPI-Host", _settings.APIHost  },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var dynamicResponse = JsonConvert.DeserializeObject<dynamic>(body);            
                
                jokesCount = (int)dynamicResponse.body;
                ViewData.Model = jokesCount;
                initializeJokesCount = true;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}