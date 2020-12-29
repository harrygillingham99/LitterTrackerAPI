using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects.InternalObjects;
using store_api.Objects.StoreObjects;

namespace store_api.CloudDatastore.DAL.Repositories
{
    public class SessionRepository : Repository, ISessionRepository
    {
        private const DbKinds.DbCollections Kind = DbKinds.DbCollections.LitterPin;

        public SessionRepository(
            ILogger<SessionRepository> logger, IOptions<ConnectionStrings> connectionStrings) : base(logger, Kind, connectionStrings.Value.ProjectName)
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