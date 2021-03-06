﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Driver.WebSite.DAL.Drivers
{
    public interface IDriversRepository
    {
        Task<Models.Driver> FindDriverByPlate(string plate);
        Task<IEnumerable<Models.Driver>> GetMostDownvotedDrivers(int limit);
    }
}