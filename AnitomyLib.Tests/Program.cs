using System;
using System.IO;
using AnimeBird.Parser;
using Newtonsoft.Json.Linq;

namespace AnitomyLib.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var file = File.ReadAllText("Data/anime.json");
            var json = JArray.Parse(file);

            foreach (var anime in json)
            {
                var fileNameParser = new FileNameParser(anime["file_name"].ToString());
                fileNameParser.Parse();

                break;
            }

            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();
        }
    }
}
