using System;
using Newtonsoft.Json.Linq;

namespace AnimeBird.Net.Rss
{
    public class FeedItem
    {

        private FeedItem(string title, string category, string link, string description, string guid, string pubDate)
        {
            Title = title;
            Category = category;
            Link = link;
            Description = description;
            Guid = guid;
            PubDate = pubDate;
        }

        public string Title { get; private set; }
        public string Category { get; private set; }
        public string Link { get; private set; }
        public string Description { get; private set; }
        public string Guid { get; private set; }
        public string PubDate { get; private set; }

        private static string GetString(JToken field)
        {
            if (field.Type == JTokenType.String) return field.ToString();

            var cdata = ((JObject) field)["#cdata-section"];

            if(cdata == null)
                throw new Exception("#cdata-section not found.");

            return cdata.ToString();
        }

        public static FeedItem Parse(JToken feedItem)
        {
            return new FeedItem(
                GetString(feedItem["title"]),
                GetString(feedItem["category"]),
                GetString(feedItem["link"]),
                GetString(feedItem["description"]),
                GetString(feedItem["guid"]),
                GetString(feedItem["pubDate"])
            );
        }
    }
}
