namespace litter_tracker.Objects.InternalObjects
{
    /*
    Config class used to hold Weather API specific appsettings at runtime. 
    Injected as IOption<OpenWeatherApi> options.
    */
    public class OpenWeatherApi
    {
        public string Key { get; set; }
        public string UrlStem { get; set; }

    }
}
