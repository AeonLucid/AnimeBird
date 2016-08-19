using System;
using System.Threading;
using System.Threading.Tasks;
using AnimeBird.Net.Rss;
using AnimeBird.Net.Rss.Services;
using AnimeBird.Util.Logging;
using Newtonsoft.Json.Linq;

namespace AnimeBird.Downloader
{
    public class Refresher
    {
        private CancellationTokenSource _refreshCancellation;

        private Task _refreshTask;

        private readonly TokyoToshoService _tokyoToshoService;

        public Refresher()
        {
            _tokyoToshoService = TokyoToshoService.GetInstance();
        }

        private async Task CheckRefresh(TaskCompletionSource<bool> firstRefreshCompleted)
        {
            while (!_refreshCancellation.IsCancellationRequested)
            {
                await Refresh();

                if (firstRefreshCompleted != null)
                {
                    firstRefreshCompleted.TrySetResult(true);
                    firstRefreshCompleted = null;
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(15), _refreshCancellation.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        private async Task Refresh()
        {
            var feed = await _tokyoToshoService.GetFeedAsync();
            var items = (JArray) feed["rss"]["channel"]["item"];

            Logger.Debug("-------------------------------------------------------------------");
            Logger.Debug($"Found {items.Count} items.");

            foreach (var item in items)
            {
                var feedItemData = await Task.Run(() => FeedItem.Parse(item));

                Logger.Debug($"Title: {feedItemData.Title}");
            }
        }

        public async Task StartRefresher()
        {
            if(_refreshTask != null)
                throw new Exception("Refresh task is already running.");

            var firstRefreshCompleted = new TaskCompletionSource<bool>();
            _refreshCancellation = new CancellationTokenSource();
            _refreshTask = CheckRefresh(firstRefreshCompleted);

            await firstRefreshCompleted.Task;
        }

        public void StopRefresher()
        {
            _refreshCancellation?.Cancel();
            _refreshTask = null;
        }

    }
}
