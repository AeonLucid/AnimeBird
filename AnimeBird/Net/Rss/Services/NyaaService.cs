namespace AnimeBird.Net.Rss.Services
{
    public class NyaaService : Service
    {
        // Search: http://www.nyaa.se/?cats=1_37&filter=2&page=rss&term=91%20days

        private NyaaService(string feedUrl) : base(feedUrl)
        {
        }

        public static NyaaService GetInstance()
        {
            return new NyaaService("http://www.nyaa.se/?page=rss&cats=1_37&filter=2");
        }
    }
}
