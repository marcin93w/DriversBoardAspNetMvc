﻿using System.Data.Entity;
using System.Linq;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public class HomePageItemsQuery : ItemsQuery
    {
        public HomePageItemsQuery(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<Item> Query
        {
            get
            {
                var source = Context.Items.Include(i => i.Author)
                    .Include(i => i.DriversOccurrences)
                    .Include(i => i.DriversOccurrences.Select(o => o.Driver));
                return from item in source
                       orderby item.DateAdded descending
                       select item;
            }
        }
    }
}
