using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using litter_tracker.CloudDatastore.DAL.Interfaces;
using litter_tracker.Objects.ApiObjects;
using litter_tracker.Objects.Helpers;
using litter_tracker.Objects.InternalObjects;
using litter_tracker.Objects.OpenWeatherApi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace litter_tracker.CloudDatastore.DAL.Repositories
{
    public class LitterTrackerRepository : Repository, ILitterTrackerRepository
    {
        private const DbKinds.DbCollections Kind = DbKinds.DbCollections.LitterPin;

        public LitterTrackerRepository(
            ILogger<LitterTrackerRepository> logger, IOptions<ConnectionStrings> connectionStrings) : base(logger, Kind,
            connectionStrings.Value.ProjectName)
        {
        }

        public async Task<List<LitterPin>> GetLitterPins()
        {
            return (await Get()).Select(x => new LitterPin
            {
                DataStoreId = x.Key.ToId(),
                MarkerLocation = JsonConvert.DeserializeObject<LatLng>(x["MarkerLocation"].StringValue),
                ImageUrls = JsonConvert.DeserializeObject<List<string>>(x["ImageUrls"].StringValue),
                CreatedByUid = x["CreatedByUid"].StringValue,
                WeatherData = JsonConvert.DeserializeObject<WeatherData>(x["WeatherData"].StringValue),
                AreaCleaned = x["AreaCleaned"].BooleanValue,
                DateCreated = x["DateCreated"].TimestampValue.ToDateTime(),
                DateLastUpdated = x["DateLastUpdated"].TimestampValue.ToDateTime(),
                LastUpdatedByUid = x["LastUpdatedByUid"].StringValue 

            }).ToList();
        }

        public async Task CreateNewLitterPin(LitterPin request)
        {
            await Insert(request);
        }

        public async Task CreateNewLitterPins(List<LitterPin> request)
        {
            foreach (var pin in request) await Insert(pin);
        }

        public async Task<LitterPin> UpdateLitterPin(LitterPin request)
        {
            var result = await Update(request, request.DataStoreId.ToKey(Kind));

            if (result)
                return (await Get(Filter.Equal("__key__", request.DataStoreId.ToKey(Kind))))
                    .Select(x => new LitterPin
                    {
                        DataStoreId = x.Key.ToId(),
                        MarkerLocation = JsonConvert.DeserializeObject<LatLng>(x["MarkerLocation"].StringValue),
                        ImageUrls = JsonConvert.DeserializeObject<List<string>>(x["ImageUrls"].StringValue),
                        CreatedByUid = x["CreatedByUid"].StringValue,
                        WeatherData = JsonConvert.DeserializeObject<WeatherData>(x["WeatherData"].StringValue),
                        AreaCleaned = x["AreaCleaned"].BooleanValue,
                        DateCreated = x["DateCreated"].TimestampValue.ToDateTime(),
                        DateLastUpdated = x["DateLastUpdated"].TimestampValue.ToDateTime(),
                        LastUpdatedByUid = x["LastUpdatedByUid"].StringValue
                    }).FirstOrDefault();

            return null;
        }

        public async Task DeleteLitterPin(long dataStoreId)
        {
            await Delete(dataStoreId.ToKey(Kind));
        }
    }
}