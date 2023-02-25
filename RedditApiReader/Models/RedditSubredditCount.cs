namespace RedditApiReader.Models
{
    public class RedditSubredditCount
    {
        public int Count { get; set; }
        public string Subreddit { get; set; }

        public RedditSubredditCount(string subreddit, int count = 1)
        {
            Subreddit = subreddit;
            Count = count;
        }
    }
}
