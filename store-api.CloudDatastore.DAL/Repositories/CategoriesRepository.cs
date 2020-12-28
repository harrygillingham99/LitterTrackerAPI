using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using store_api.CloudDatastore.DAL.Interfaces;
using store_api.Objects;
using store_api.Objects.Helpers;
using store_api.Objects.InternalObjects;
using store_api.Objects.StoreObjects;
using static store_api.Objects.InternalObjects.DbKinds;

namespace store_api.CloudDatastore.DAL.Repositories
{
    public class CategoriesRepository : Repository, ICategoriesRepository
    {
        private const DbCollections Kind = DbCollections.Categories;
        public CategoriesRepository(ILogger<Repository> logger, IOptions<ConnectionStrings> connectionStrings) : base(logger, Kind, connectionStrings.Value.ProjectName)
        {
        }
        public async Task<IEnumerable<Categories>> GetCategories()
        {
            var result = await Get();
            return result.Select(entity => new Categories
            {
                DataStoreId = entity.Key.ToId(),
                Id = (int)entity.Properties["Id"].IntegerValue,
                Category = entity.Properties["Category"]
                    .StringValue
            });
        }

        public Task<bool> AddCategories(Categories categoryToAdd)
        {
            return Insert(categoryToAdd);
        }

        public Task<bool> UpdateCategory(Categories updatedCategory)
        {
            return Update(updatedCategory, updatedCategory.DataStoreId.ToKey(Kind));
        }

        public Task DeleteCategory(long key)
        { 
             return Delete(key.ToKey(Kind));
        }
    }
}
