using System.Collections.Generic;
using System.Threading.Tasks;
using store_api.Objects;
using store_api.Objects.StoreObjects;

namespace store_api.CloudDatastore.DAL.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Categories>> GetCategories();
        Task<bool> AddCategories(Categories categoryToAdd);
        Task<bool> UpdateCategory(Categories updatedCategory);
        Task DeleteCategory(long key);
    }
}