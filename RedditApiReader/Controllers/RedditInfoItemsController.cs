using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedditApiReader.Data;
using RedditApiReader.Models;

namespace RedditApiReader.Controllers
{
    public class RedditInfoItemsController : Controller
    {
        private readonly RedditApiReaderContext _context;
        private static int desiredCount = 10;

        public RedditInfoItemsController(RedditApiReaderContext context)
        {
            _context = context;
        }

        // GET: RedditInfoItems/ByDomain
        public IActionResult ByDomain()
        {
            var outputList = _context.RedditInfoItem.GroupBy(x => x.Domain)
                                          .Select(group => new
                                          {
                                              Domain = group.Key,
                                              Count = group.Count()
                                          }).OrderByDescending(x => x.Count)
                                          .ToList();

            List<RedditCountItem> returnList = new List<RedditCountItem>();
            for (int i = 0; i < desiredCount; i++)
            {
                returnList.Add(new RedditCountItem(outputList[i].Domain ?? "Domain Missing", outputList[i].Count));
            }

            return RedditStatistics(returnList, "Domain");
        }

        // GET: RedditInfoItems/RedditStatistics
        public IActionResult RedditStatistics(List<RedditCountItem> items, string analysisType)
        {
            int count = _context.RedditInfoItem.Count();
            RedditDisplayViewer output = new RedditDisplayViewer(items, analysisType, count);
            return View("RedditStatistics", output);
        }

        // GET: RedditInfoItems/BySubreddit
        public IActionResult BySubreddit()
        {
            var outputList = _context.RedditInfoItem.GroupBy(x => x.Subreddit)
                                          .Select(group => new
                                          {
                                              Subreddit = group.Key,
                                              Count = group.Count()
                                          }).OrderByDescending(x => x.Count)
                                          .ToList();

            List<RedditCountItem> returnList = new List<RedditCountItem>();
            for (int i = 0; i < desiredCount; i++)
            {
                returnList.Add(new RedditCountItem(outputList[i].Subreddit ?? "Subreddit Missing", outputList[i].Count));
            }

            return RedditStatistics(returnList, "Subreddit");
        }

        // GET: RedditInfoItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.RedditInfoItem.ToListAsync());
        }
    }
}
