using System.Threading.Tasks;

namespace store_api.CloudDatastore.DAL.Interfaces
{
    public interface IAdminRepository
    {
        Task<bool> IsAdminUser(string uid);
    }
}