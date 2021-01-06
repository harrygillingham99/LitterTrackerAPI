using System.Threading.Tasks;
using litter_tracker.Objects.ApiObjects;
using litter_tracker.Objects.OpenWeatherApi;

namespace litter_tracker.Services.OpenWeatherApi
{
    public interface IOpenWeatherServiceAgent
    {
        Task<WeatherData> GetWeatherForPin(LatLng location);
    }
}