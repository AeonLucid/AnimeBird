using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnimeBird.Net.Rss.Services
{
    public abstract class Service
    {
        private readonly string _feedUrl;
        private readonly HttpClient _httpClient;

        protected Service(string feedUrl)
        {
            _feedUrl = feedUrl;
            _httpClient = new HttpClient();
        }

        public async Task<JObject> GetFeedAsync()
        {
            var feed = await _httpClient.GetAsync(_feedUrl);
            var feedData = await feed.Content.ReadAsStringAsync();
            var feedJsonData = JsonConvert.SerializeXNode(XDocument.Parse(feedData));

            return JObject.Parse(feedJsonData);
        }

    }
}
