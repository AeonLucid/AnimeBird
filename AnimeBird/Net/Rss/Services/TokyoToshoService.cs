namespace AnimeBird.Net.Rss.Services
{
    public class TokyoToshoService : Service
    {

        private TokyoToshoService(string feedUrl) : base(feedUrl)
        {
        }

        public static TokyoToshoService GetInstance()
        {
            return new TokyoToshoService("http://tokyotosho.info/rss.php?filter=1,11&zwnj=0");
        }

    }
}
