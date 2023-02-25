namespace RedditApiReader.Models
{
    public class RedditCountItem
    {
        public int Count { get; set; }
        public string Field { get; set; }

        public RedditCountItem(string field, int count = 1)
        {
            Field = field;
            Count = count;
        }

    }
}
