using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.WebSite.DAL
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
    }
}
