using System.Collections.Generic;
using System.Threading.Tasks;
using store_api.Objects.StoreObjects;

namespace store_api.CloudDatastore.DAL.Interfaces
{
    public interface ILitterTrackerRepository
    {
        Task<IEnumerable<LitterPin>> GetCurrentBasket(string requestUid);
    }
}