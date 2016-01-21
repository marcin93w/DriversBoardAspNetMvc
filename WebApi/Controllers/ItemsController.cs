using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Driver.Common.Models;

namespace WebApi.Controllers
{
    public class ItemsController : ApiController
    {
        private ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public int RateUp(int id)
        {
            return 0;
        }

        [HttpGet]
        public int RateDown(int id)
        {
            return 0;
        }

        [HttpGet]
        public int ClearRating(int id)
        {
            return 0;
        }
    }
}
