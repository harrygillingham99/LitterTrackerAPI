using System.Collections.Generic;
using System.Threading.Tasks;
using litter_tracker.Objects.StoreObjects;

namespace litter_tracker.CloudDatastore.DAL.Interfaces
{
    public interface ILitterTrackerRepository
    {
        Task<IEnumerable<LitterPin>> GetCurrentBasket(string requestUid);
    }
}