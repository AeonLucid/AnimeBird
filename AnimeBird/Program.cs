using System;
using System.Threading;

namespace AnimeBird
{
    public class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
            Console.Title = "AnimeBird";
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                QuitEvent.Set();
                eventArgs.Cancel = true;
            };

            var environment = new Environment();

            // Run program
            environment.Run(args).GetAwaiter().GetResult();
            // Wait until close event
            QuitEvent.WaitOne();
            // Shutdown program
            environment.Shutdown();
        }
    }
}

/*
    GET /rss.php?filter=1%2C11&zwnj=0 HTTP/1.1
    Host: tokyotosho.info
    User-Agent: Taiga/1.2
    Accept: (all)
    Accept-Encoding: gzip

    GET /?cats=1_37&filter=2&page=rss&term=91%20days HTTP/1.1
    Host: www.nyaa.se
    User-Agent: Taiga/1.2
    Accept: (all)
    Accept-Encoding: gzip
*/
