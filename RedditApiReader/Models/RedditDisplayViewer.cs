namespace RedditApiReader.Models
{
    public class RedditDisplayViewer
    {
        public int Count { get; set; }
        public string AnalysisType { get; set; }
        public List<RedditCountItem> Items { get; set; }

        public RedditDisplayViewer(List<RedditCountItem> items, string name, int count) 
        {
            Items = items;
            Count = count;
            AnalysisType = name;
        }
    }
}
