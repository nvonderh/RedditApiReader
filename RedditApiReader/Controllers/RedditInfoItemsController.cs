using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
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
            List<RedditDomainCount> returnList = new List<RedditDomainCount>();
            for (int i = 0; i < desiredCount; i++)
            {
                returnList.Add(new RedditDomainCount(outputList[i].Domain ?? "Domain Missing", outputList[i].Count));
            }
            return View(returnList);
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
            List<RedditSubredditCount> returnList = new List<RedditSubredditCount>();
            for (int i = 0; i < desiredCount; i++)
            {
                returnList.Add(new RedditSubredditCount(outputList[i].Subreddit ?? "Subreddit Missing", outputList[i].Count));
            }
            return View(returnList);
        }

        // GET: RedditInfoItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.RedditInfoItem.ToListAsync());
        }

        //// GET: RedditInfoItems/Retrieve
        //public async Task<IActionResult> Retrieve()
        //{
        //    List<RedditInfoItem> returnedList = new List<RedditInfoItem>();
        //    string url = OAUTH_API_DOMAIN + "/new";

        //    var task = client.GetAsync(url);
        //    var response = task.Result;

        //    if (response.StatusCode != System.Net.HttpStatusCode.OK)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    if (response != null)
        //    {
        //        var jsonString = await response.Content.ReadAsStringAsync();
        //        var jsonObject = Newtonsoft.Json.Linq.JObject.Parse(jsonString);
        //        returnedList.AddRange(GetRedditInfoItems(jsonObject));

        //        _context.AddRange(returnedList);
        //        await _context.SaveChangesAsync();
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        private bool RedditInfoItemExists(int id)
        {
          return _context.RedditInfoItem.Any(e => e.Id == id);
        }
    }
}
