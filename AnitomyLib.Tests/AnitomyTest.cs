using System.IO;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AnitomyLib.Tests
{
    public class AnitomyTest
    {
        [Fact]
        public void TestParser()
        {
            var file = File.ReadAllText("Data/anime.json");
            var json = JArray.Parse(file);

            foreach (var anime in json)
            {
                var anitomy = new Anitomy();;
                anitomy.Parse(anime["file_name"].ToString());

                break;
            }
        }
    }
}
