﻿using System.Collections.Generic;
using litter_tracker.Objects.OpenWeatherApi;

namespace litter_tracker.Objects.StoreObjects
{
    public class LitterPin : DataStoreItem
    {
        public LatLng MarkerLocation { get; set; }
        public List<string> ImageUrls { get; set; }
        public WeatherData WeatherData { get; set; }
        public string CreatedByUid { get; set; }
        public bool AreaCleaned { get; set; }

    }
}