using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace RedditApiReader.Models
{
    public class RedditInfoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Subreddit { get; set; }
        public bool HasSelfText { get; set; }
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }
        public string? Author { get; set; }
        public string? Domain { get; set; }
        public string? Url { get; set; }

        public static List<RedditInfoItem> GetRedditInfoItems(JObject jsonObject)
        {
            List<RedditInfoItem> intoItemList = new List<RedditInfoItem>();
            var redditList = jsonObject.SelectToken("data").SelectToken("children").SelectMany(x => x.SelectTokens("data"));
            foreach (var item in redditList)
            {
                if (item == null) continue;
                intoItemList.Add(ConvertJsonToRedditInfoItem(item));
            }
            return intoItemList;
        }

        public static RedditInfoItem ConvertJsonToRedditInfoItem(JToken token)
        {
            RedditInfoItem ret = new RedditInfoItem();
            ret.Author = token.SelectToken("author").ToString();
            var ticks = long.Parse(token.SelectToken("created").ToString());
            ret.Created = new DateTime(ticks);
            ret.Domain = token.SelectToken("domain").ToString();
            ret.HasSelfText = token.SelectToken("selftext").ToString().Length > 0;
            ret.Subreddit = token.SelectToken("subreddit").ToString();
            ret.Title = token.SelectToken("title").ToString();
            ret.Url = token.SelectToken("url").ToString();
            return ret;
        }
    }
}
