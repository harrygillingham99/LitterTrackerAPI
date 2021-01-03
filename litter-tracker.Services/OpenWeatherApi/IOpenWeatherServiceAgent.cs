using System.Threading.Tasks;
using litter_tracker.Objects.OpenWeatherApi;
using litter_tracker.Objects.StoreObjects;

namespace litter_tracker.Services.OpenWeatherApi
{
    public interface IOpenWeatherServiceAgent
    {
        Task<WeatherData> GetWeatherForPin(LatLng location);
    }
}