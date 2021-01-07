using System.Collections.Generic;
using System.Threading.Tasks;
using litter_tracker.Objects.ApiObjects;

namespace litter_tracker.CloudDatastore.DAL.Interfaces
{
    public interface ILitterTrackerRepository
    {
        public Task<List<LitterPin>> GetLitterPins();

        Task<LitterPin> CreateNewLitterPin(LitterPin request, string requestUid);

        Task<List<LitterPin>> CreateNewLitterPins(List<LitterPin> request, string requestUid);

        Task<LitterPin> UpdateLitterPin(LitterPin request, string requestUid);
        Task DeleteLitterPin(long dataStoreId);
    }
}