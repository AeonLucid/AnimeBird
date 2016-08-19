using System.Threading.Tasks;
using AnimeBird.Downloader;
using AnimeBird.Util.Logging;

namespace AnimeBird
{
    public class Environment
    {
        private readonly Refresher _refresher;

        public Environment()
        {
            _refresher = new Refresher();
        }

        public async Task Run(string[] args)
        {
            Logger.Info("Starting up AnimeBird");

            await _refresher.StartRefresher();
        }

        public void Shutdown()
        {
            Logger.Warn("Shutting down AnimeBird");

            _refresher.StopRefresher();
        }

    }
}
