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
            return (await Get()).Select(MapEntityToLitterPin).ToList();
        }

        public async Task<LitterPin> CreateNewLitterPin(LitterPin request, string requestUid)
        {
            request = request.EnsureObjectValid(requestUid, CreatePin);
            await Insert(request);
            return (await Get(Filter.Equal("MarkerLocation", JsonConvert.SerializeObject(request.MarkerLocation))))
                .Select(MapEntityToLitterPin).FirstOrDefault();
        }

        public async Task<List<LitterPin>> CreateNewLitterPins(List<LitterPin> request, string requestUid)
        {
            var createdPins = new List<LitterPin>();

            foreach (var pin in request)
            {
                var pinToCreate = pin.EnsureObjectValid(requestUid, CreatePin);

                await Insert(pinToCreate);

                createdPins.Add((await Get(Filter.Equal("MarkerLocation", JsonConvert.SerializeObject(pin.MarkerLocation))))
                    .Select(MapEntityToLitterPin).FirstOrDefault());
            }

            return createdPins;
        }

        public async Task<LitterPin> UpdateLitterPin(LitterPin request, string requestUid)
        {
            request = request.EnsureObjectValid(requestUid, UpdatePin);

            var result = await Update(request, request.DataStoreId.ToKey(Kind));

            if (result)
                return (await Get(Filter.Equal("__key__", request.DataStoreId.ToKey(Kind))))
                    .Select(MapEntityToLitterPin).FirstOrDefault();

            return null;
        }

        public async Task DeleteLitterPin(long dataStoreId)
        {
            await Delete(dataStoreId.ToKey(Kind));
        }

        public async Task<UserStatistics> GetStatsForUser(string requestUid)
        {
            var result = (await Get()).Select(MapEntityToLitterPin).ToList();
            return new UserStatistics
            {
                AreasCleared = result.Count(x => x.LastUpdatedByUid == requestUid && x.AreaCleaned),
                PinsCreated = result.Count(x => x.CreatedByUid == requestUid)
            };

        }

        private LitterPin MapEntityToLitterPin(Entity entity) => new LitterPin
        {
            DataStoreId = entity.Key.ToId(),
            MarkerLocation = JsonConvert.DeserializeObject<LatLng>(entity["MarkerLocation"].StringValue),
            ImageUrls = JsonConvert.DeserializeObject<List<string>>(entity["ImageUrls"].StringValue),
            CreatedByUid = entity["CreatedByUid"].StringValue,
            WeatherData = JsonConvert.DeserializeObject<WeatherData>(entity["WeatherData"].StringValue),
            AreaCleaned = entity["AreaCleaned"].BooleanValue,
            DateCreated = entity["DateCreated"].TimestampValue.ToDateTime(),
            DateLastUpdated = entity["DateLastUpdated"].TimestampValue.ToDateTime(),
            LastUpdatedByUid = entity["LastUpdatedByUid"].StringValue
        };
    }
}