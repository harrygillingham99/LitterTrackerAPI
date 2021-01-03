using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using litter_tracker.Objects.Helpers;
using litter_tracker.Objects.OpenWeatherApi;
using litter_tracker.Objects.StoreObjects;
using Microsoft.Extensions.Options;
using RestSharp;

namespace litter_tracker.Services.OpenWeatherApi
{
    public class OpenWeatherServiceAgent : IOpenWeatherServiceAgent
    {
        private readonly string _apiKey;
        private readonly IRestClient _client;
        private const string _urlParams = "/weather?lat={0}&lon={1}&appid={2}&units=metric";

        public OpenWeatherServiceAgent(IOptions<Objects.InternalObjects.OpenWeatherApi> options)
        {
            _apiKey = options.Value.Key;
            _client = new RestClient(options.Value.UrlStem);
        }

        public async Task<WeatherData> GetWeatherForPin(LatLng location)
        {
            var request = new RestRequest(string.Format(_urlParams, location.Latitude, location.Longitude, _apiKey));
            return  (await _client.ExecuteAsync<OpenWeatherResponseRoot>(request)).Data.MapToWeatherData();
        }
    }
}
