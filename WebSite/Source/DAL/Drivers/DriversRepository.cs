using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Driver.WebSite.DAL.Drivers
{
    public class DriversRepository : IDriversRepository
    {
        private readonly ApplicationDbContext _context;

        public DriversRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Driver> FindDriverByPlate(string plate)
        {
            var query = from row in _context.DriverOccurrences
                where row.Driver.Plate == plate.Replace(" ", string.Empty)
                select row.Driver;

            return (await query.ToArrayAsync()).FirstOrDefault();
        }

        public async Task<IEnumerable<Models.Driver>> GetMostDownvotedDrivers(int limit)
        {
            return await (from row in _context.Set<Models.Driver>()
                    orderby row.DownVotesCount descending 
                    select row).Take(limit).ToArrayAsync();
        }
    }
}
