using System.Linq;
using litter_tracker.Objects.OpenWeatherApi;

namespace litter_tracker.Objects.Helpers
{
    public static class OpenWeatherDataMapper
    {
        public static WeatherData MapToWeatherData(this OpenWeatherResponseRoot response)
        {
            return new WeatherData
            {
                Temperature = (decimal) response.main.temp,
                WeatherDescription = response.weather.Select(x => x.description).FirstOrDefault(),
                WindSpeed = (decimal) response.wind.speed,
                WindDirection = (decimal) response.wind.deg
            };

        }
    }
}
