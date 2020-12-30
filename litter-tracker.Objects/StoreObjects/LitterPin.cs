using System.Collections.Generic;

namespace litter_tracker.Objects.StoreObjects
{
    public class LitterPin : DataStoreItem
    {
        public LatLng MarkerLocation { get; set; }
        public List<string> ImageUrls { get; set; }
        public string CreatedByUid { get; set; }

    }

    public class LatLng
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}