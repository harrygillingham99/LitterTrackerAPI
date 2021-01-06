using System.Collections.Generic;
using System.Threading.Tasks;
using litter_tracker.Objects.ApiObjects;
using litter_tracker.Services.OpenWeatherApi;

namespace store_api.Helpers
{
    public static class LitterPinHelper
    {
        public static async Task<LitterPin> EnsureWeatherData(this LitterPin pin, IOpenWeatherServiceAgent service)
        {
            pin.WeatherData = await service.GetWeatherForPin(pin.MarkerLocation);
            return pin;
        }

        public static async Task<List<LitterPin>> EnsureWeatherData(this List<LitterPin> pins, IOpenWeatherServiceAgent service)
        {
            List<LitterPin> updatedPins = new List<LitterPin>();

            foreach (var pin in pins)
            {
                updatedPins.Add(await pin.EnsureWeatherData(service));
            }
            return updatedPins;
        }
    }
}
