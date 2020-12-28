using System.Collections.Generic;
using System.Threading.Tasks;
using store_api.Objects;
using store_api.Objects.StoreObjects;

namespace store_api.CloudDatastore.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<bool> UpdateProduct(Product product);
        Task<bool> InsertProduct(Product product);
        Task DeleteProduct(long key);
    }
}