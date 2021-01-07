using System;
using System.Collections.Generic;
using System.Text;
using litter_tracker.Objects.ApiObjects;
using litter_tracker.Objects.InternalObjects;
using litter_tracker.Objects.OpenWeatherApi;

namespace litter_tracker.Objects.Helpers
{
    public static class LitterPinHelper
    {
        public static LitterPin EnsureObjectValid(this LitterPin pin, string requestUid, DbKinds.PinOperationType operation)
        {
            var now = DateTime.Now;

            if (pin.ImageUrls == null)
            {
                pin.ImageUrls = new List<string>();
            }

            if (pin.WeatherData == null)
            {
                pin.WeatherData = new WeatherData();
            }

            if (pin.MarkerLocation == null)
            {
                pin.MarkerLocation = new LatLng();
            }

            switch (operation)
            {
                case DbKinds.PinOperationType.CreatePin:
                    pin.DateCreated = now;
                    pin.CreatedByUid = requestUid;
                    pin.DateLastUpdated = now;
                    pin.LastUpdatedByUid = requestUid;
                    break;
                case DbKinds.PinOperationType.UpdatePin:
                    pin.DateLastUpdated = now;
                    pin.LastUpdatedByUid = requestUid;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, "Invalid Operation Type");
            }
            return pin;
        }
    }
}
