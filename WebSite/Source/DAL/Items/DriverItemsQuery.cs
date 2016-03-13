using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Html;
using Driver.WebSite.Models;

namespace Driver.WebSite.DAL.Items
{
    public class DriverItemsQuery : ItemsQuery
    {
        private readonly string _driverPlate;
        private readonly int? _driverId;

        public DriverItemsQuery(ApplicationDbContext context, int driverId) : base(context)
        {
            _driverId = driverId;
        }
        public DriverItemsQuery(ApplicationDbContext context, string driverPlate) : base(context)
        {
            _driverPlate = driverPlate;
        }

        protected override IQueryable<Item> Query
        {
            get
            {
                IQueryable<Item> query;
                if (_driverId.HasValue)
                {
                    query = from item in Context.Items
                            join driverOccurence in Context.DriverOccurrences
                            on item.Id equals driverOccurence.Item.Id
                            where driverOccurence.Driver.Id == _driverId.Value
                            select item;
                }
                else
                {
                    query = from item in Context.Items
                            join driverOccurence in Context.DriverOccurrences
                            on item.Id equals driverOccurence.Item.Id
                            where driverOccurence.Driver.Plate == _driverPlate
                            select item;
                }

                query = query.OrderByDescending(i => i.DateAdded);
                return query.Include(i => i.Author)
                    .Include(i => i.DriversOccurrences)
                    .Include(i => i.DriversOccurrences.Select(o => o.Driver));
            }
        }
    }
}
