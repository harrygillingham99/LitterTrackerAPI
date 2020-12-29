using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using litter_tracker.CloudDatastore.DAL.Interfaces;
using litter_tracker.Objects.InternalObjects;
using litter_tracker.Objects.StoreObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace litter_tracker.CloudDatastore.DAL.Repositories
{
    public class LitterTrackerRepository : Repository, ILitterTrackerRepository
    {
        private const DbKinds.DbCollections Kind = DbKinds.DbCollections.LitterPin;

        public LitterTrackerRepository(
            ILogger<LitterTrackerRepository> logger, IOptions<ConnectionStrings> connectionStrings) : base(logger, Kind, connectionStrings.Value.ProjectName)
        {
        }


        public async Task<IEnumerable<LitterPin>> GetCurrentBasket(string requestUid)
        {
            return (await Get()).Select(x => new LitterPin
            {

            });
        }
    }
}