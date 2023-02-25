using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RedditApiReader.Models;

namespace RedditApiReader.Data
{
    public class RedditApiReaderContext : DbContext
    {
        public RedditApiReaderContext (DbContextOptions<RedditApiReaderContext> options)
            : base(options)
        {
        }

        public DbSet<RedditApiReader.Models.RedditInfoItem> RedditInfoItem { get; set; } = default!;
    }
}
