using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedditApiReader.Controllers;
using RedditApiReader.Data;
using Moq;
using RedditApiReader.Models;
using Microsoft.EntityFrameworkCore.InMemory;

namespace RedditApiReader.Tests.Controllers
{
    [TestClass]
    public class RedditInfoItemsControllerTest
    {
        [TestMethod]
        public void TestHomeView()
        {
            var options = new DbContextOptionsBuilder<RedditApiReaderContext>().Options;
            RedditApiReaderContext context = new RedditApiReaderContext(options);

            var controller = new RedditInfoItemsController(context);
            var result = controller.Help() as ViewResult;
            Assert.AreEqual("Help", result.ViewName);
        }

        [TestMethod]
        public void TestIndexView()
        {
            var options = new DbContextOptionsBuilder<RedditApiReaderContext>()
                .UseInMemoryDatabase(databaseName: "RedditApiReaderDatabase")
                .Options;
            RedditApiReaderContext context = new RedditApiReaderContext(options);

            context.Add(new RedditInfoItem());
            context.SaveChanges();
            var controller = new RedditInfoItemsController(context);

            var result = controller.Index().Result as ViewResult;
            Assert.IsNull(result.ViewName);
            Assert.AreEqual(1, (result.Model as List<RedditInfoItem>).Count);

            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void TestByDomainView()
        {
            var options = new DbContextOptionsBuilder<RedditApiReaderContext>()
                .UseInMemoryDatabase(databaseName: "RedditApiReaderDatabase")
                .Options;
            RedditApiReaderContext context = new RedditApiReaderContext(options);

            context.Add(new RedditInfoItem());
            context.SaveChanges();
            var controller = new RedditInfoItemsController(context);

            var result = controller.ByDomain() as ViewResult;
            Assert.AreEqual("RedditStatistics", result.ViewName);
            Assert.AreEqual("Domain", (result.Model as RedditDisplayViewer).AnalysisType);
            Assert.AreEqual(1, (result.Model as RedditDisplayViewer).Count);
            Assert.AreEqual(1, (result.Model as RedditDisplayViewer).Items.Count());


            for (int i = 0; i < 10; i++)
            {
                context.Add(new RedditInfoItem() { Domain = "domain" + i });
            }
            context.SaveChanges();
            controller = new RedditInfoItemsController(context);

            result = controller.ByDomain() as ViewResult;
            Assert.AreEqual("RedditStatistics", result.ViewName);
            Assert.AreEqual(11, (result.Model as RedditDisplayViewer).Count);
            Assert.AreEqual(10, (result.Model as RedditDisplayViewer).Items.Count());

            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void TestBySubredditView()
        {
            var options = new DbContextOptionsBuilder<RedditApiReaderContext>()
                .UseInMemoryDatabase(databaseName: "RedditApiReaderDatabase")
                .Options;
            RedditApiReaderContext context = new RedditApiReaderContext(options);

            context.Add(new RedditInfoItem());
            context.SaveChanges();
            var controller = new RedditInfoItemsController(context);

            var result = controller.BySubreddit() as ViewResult;
            Assert.AreEqual("RedditStatistics", result.ViewName);
            Assert.AreEqual("Subreddit", (result.Model as RedditDisplayViewer).AnalysisType);
            Assert.AreEqual(1, (result.Model as RedditDisplayViewer).Count);
            Assert.AreEqual(1, (result.Model as RedditDisplayViewer).Items.Count());

            for (int i = 0; i < 10; i++)
            {
                context.Add(new RedditInfoItem() { Subreddit = "subreddit" + i});
            }
            context.SaveChanges();
            controller = new RedditInfoItemsController(context);

            result = controller.BySubreddit() as ViewResult;
            Assert.AreEqual("RedditStatistics", result.ViewName);
            Assert.AreEqual(11, (result.Model as RedditDisplayViewer).Count);
            Assert.AreEqual(10, (result.Model as RedditDisplayViewer).Items.Count());

            context.Database.EnsureDeleted();
        }
    }
}
