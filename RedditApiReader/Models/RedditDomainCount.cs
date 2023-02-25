namespace RedditApiReader.Models
{
    public class RedditDomainCount
    {
        public int Count { get; set; }
        public string Domain { get; set; }

        public RedditDomainCount(string domain, int count = 1)
        {
            Domain = domain;
            Count = count;
        }
    }
}
