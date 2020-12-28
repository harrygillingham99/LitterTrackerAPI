using System.Collections.Generic;
using System.Threading.Tasks;
using store_api.Objects;
using store_api.Objects.StoreObjects;

namespace store_api.CloudDatastore.DAL.Interfaces
{
    public interface ISessionRepository
    {
        Task<Basket> GetCurrentBasket(string requestUid);
        Task<bool> UpdateBasket(Basket basketToUpdate);
        Task<bool> OrderItems(Basket basketToOrder);
        Task<IEnumerable<Basket>> GetHistoricOrders(string requestUid);
    }
}