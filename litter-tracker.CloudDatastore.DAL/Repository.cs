using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Google.Protobuf.WellKnownTypes;
using litter_tracker.Objects.Helpers;
using litter_tracker.Objects.StoreObjects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static litter_tracker.Objects.InternalObjects.DbKinds;
using Type = System.Type;
using Value = Google.Cloud.Datastore.V1.Value;

namespace litter_tracker.CloudDatastore.DAL
{
    public abstract class Repository
    {
        private readonly string[] _dataStorePropertiesToIgnore = {"DataStoreId"};
        private readonly Type[] _typesToJsonSerialize = {typeof(List<string>), typeof(LatLng)};
        private readonly DatastoreDb _db;
        private readonly ILogger<Repository> _logger;
        private readonly DbCollections _kind;

        protected Repository(ILogger<Repository> logger, DbCollections kind, string projectName)
        {
            _logger = logger;
            _kind = kind;
            _db = DatastoreDb.Create(projectName);
        }

        protected async Task<IEnumerable<Entity>> Get(Filter filter = null)
        {
            try
            {
                var query = new Query(_kind.GetDescription())
                {
                    Filter = filter,
                };
                var result = (await _db.RunQueryAsync(query)).Entities;
                return result;
            }
            catch (Exception ex)
            {
                var exceptionMsg = $"{GetType().FullName}.Get experienced a {ex.GetType()}";
                _logger.LogError(ex, exceptionMsg);
                throw new Exception(exceptionMsg, ex);
            }
        }

        protected async Task Delete(Key key)
        {
            try
            {
                await _db.DeleteAsync(key);
            }
            catch (Exception ex)
            {
                var exceptionMsg = $"{GetType().FullName}.Delete experienced a {ex.GetType()}";
                _logger.LogError(ex, exceptionMsg);
                throw new Exception(exceptionMsg, ex);
            }
        }

        protected async Task<bool> Insert<T>(T item)
        {
            try
            {
                var keyFactory = _db.CreateKeyFactory(_kind.GetDescription());
                using var tran = await _db.BeginTransactionAsync();
                var entityToInsert = new Entity
                {
                    Key = keyFactory.CreateIncompleteKey()
                };

                tran.Insert(MapToEntity(entityToInsert, item));

                return (await tran.CommitAsync()).IndexUpdates > 0;
            }
            catch (Exception ex)
            {
                var exceptionMsg = $"{GetType().FullName}.Insert experienced a {ex.GetType()}";
                _logger.LogError(ex, exceptionMsg);
                throw new Exception(exceptionMsg, ex);
            }
        }

        protected async Task<bool> Update<T>(T updatedItem, Key dataStoreKey)
        {
            try
            {
                using var tran = await _db.BeginTransactionAsync();

                var entityToUpdate = new Entity
                {
                    Key = dataStoreKey
                };

                tran.Update(MapToEntity(entityToUpdate, updatedItem));

                return (await tran.CommitAsync()).IndexUpdates > 0;
            }
            catch (Exception ex)
            {
                var exceptionMsg = $"{GetType().FullName}.Update experienced a {ex.GetType()}";
                _logger.LogError(ex, exceptionMsg);
                throw new Exception(exceptionMsg, ex);
            }
        }

        private Entity MapToEntity<T>(Entity entityWithKey, T item)
        {
            foreach (var propertyInfo in item.GetType().GetProperties()
                .Where(x => !_dataStorePropertiesToIgnore.Contains(x.Name)))
                entityWithKey.Properties.Add(propertyInfo.Name, GetValueForItem(propertyInfo, item));

            return entityWithKey;
        }

        private bool ShouldSerializeAsJson(PropertyInfo property)
        {
            return _typesToJsonSerialize.Contains(property.PropertyType);
        }

        private Value GetValueForItem<T>(PropertyInfo propertyInfo, T item)
        {
            if (ShouldSerializeAsJson(propertyInfo))
            {
                return new Value
                {
                    StringValue = JsonConvert.SerializeObject(propertyInfo.GetValue(item))

                };
            }
               
            var propertyValue = propertyInfo?.GetValue(item)?.ToString();

            if (propertyValue == null)
                return new Value
                {
                    NullValue = NullValue.NullValue
                };
            if (int.TryParse(propertyValue, out var integerResult))
                return new Value
                {
                    IntegerValue = integerResult
                };

            if (double.TryParse(propertyValue, out var doubleResult))
                return new Value
                {
                    DoubleValue = doubleResult
                };
            if (bool.TryParse(propertyValue, out var booleanResult))
                return new Value
                {
                    BooleanValue = booleanResult
                };

            if (DateTime.TryParse(propertyValue, out var dateTimeResult))
                return new Value
                {
                    TimestampValue = new Timestamp(Timestamp.FromDateTime(dateTimeResult.ToUniversalTime()))
                };

            return new Value
            {
                StringValue = propertyValue
            };
        }
    }
}
