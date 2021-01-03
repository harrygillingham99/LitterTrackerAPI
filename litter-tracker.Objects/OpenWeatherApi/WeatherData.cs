namespace litter_tracker.Objects.OpenWeatherApi
{
    public class WeatherData
    {
        public decimal Temperature { get; set; }
        public string WeatherDescription { get; set; }
        public decimal WindSpeed { get; set; }
        public decimal WindDirection { get; set; }

    }
}