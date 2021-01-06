using System.Collections.Generic;
using System.Threading.Tasks;
using litter_tracker.Objects.ApiObjects;

namespace litter_tracker.CloudDatastore.DAL.Interfaces
{
    public interface ILitterTrackerRepository
    {
        public Task<List<LitterPin>> GetLitterPins();

        Task CreateNewLitterPin(LitterPin request);

        Task CreateNewLitterPins(List<LitterPin> request);

        Task<LitterPin> UpdateLitterPin(LitterPin request);
        Task DeleteLitterPin(long dataStoreId);
    }
}