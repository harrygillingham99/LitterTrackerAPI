using System;
using System.Collections.Generic;

namespace store_api.Objects.StoreObjects
{
    public class LitterPin : DataStoreItem
    {
        public List<int> ProductAndQuantity { get; set; }
        public bool HasPlacedOrder { get; set; }

        public string UserUid { get; set; }

        public DateTime? DateOrdered { get; set; }
    }
}