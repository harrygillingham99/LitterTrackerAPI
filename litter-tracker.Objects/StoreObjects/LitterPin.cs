using System;
using System.Collections.Generic;

namespace store_api.Objects.StoreObjects
{
    public class LitterPin : DataStoreItem
    {
        public LatLng MarkerLocation { get; set; }
        public List<string> ImageUrls { get; set; }
        public string CreatedByUid { get; set; }

    }

    public class LatLng
    {
        public long Latitude { get; set; }
        public long Longitude { get; set; }
    }
}