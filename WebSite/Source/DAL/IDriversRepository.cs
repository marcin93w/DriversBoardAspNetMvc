using System.Threading.Tasks;

namespace Driver.WebSite.DAL
{
    public interface IDriversRepository
    {
        Task<Models.Driver> FindDriverByPlate(string plate);
    }
}