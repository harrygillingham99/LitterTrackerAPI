using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects;
using store_api.Objects.InternalObjects;

namespace store_api.CloudDatastore.DAL.Repositories
{
    public class AdminRepository : Repository, IAdminRepository
    {
        private const DbKinds.DbCollections Kind = DbKinds.DbCollections.AdminUser;
        public AdminRepository(ILogger<Repository> logger, IOptions<ConnectionStrings> connectionStrings) : base(logger, Kind, connectionStrings.Value.ProjectName)
        {
        }

        public async Task<bool> IsAdminUser(string uid)
        {
            return (await Get()).Any(x =>
                string.Equals(x.Properties["Uid"].StringValue, uid, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
