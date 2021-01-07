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
using static litter_tracker.Objects.InternalObjects.DbKinds.PinOperationType;

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

        public async Task<LitterPin> CreateNewLitterPin(LitterPin request, string requestUid)
        {
            request = request.EnsureObjectValid(requestUid, CreatePin);
            await Insert(request);
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
        }

        public async Task<List<LitterPin>> CreateNewLitterPins(List<LitterPin> request, string requestUid)
        {
            var createdPins = new List<LitterPin>();

            foreach (var pin in request)
            {
                var pinToCreate = pin.EnsureObjectValid(requestUid, CreatePin);

                await Insert(pinToCreate);

                createdPins.Add((await Get(Filter.Equal("__key__", pin.DataStoreId.ToKey(Kind))))
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
                    }).FirstOrDefault());
            }

            return createdPins;
        }

        public async Task<LitterPin> UpdateLitterPin(LitterPin request, string requestUid)
        {
            request = request.EnsureObjectValid(requestUid, UpdatePin);

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