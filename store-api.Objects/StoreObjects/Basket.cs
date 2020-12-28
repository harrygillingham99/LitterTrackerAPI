using System;
using System.Collections.Generic;
using NJsonSchema.Annotations;

namespace store_api.Objects.StoreObjects
{
    [JsonSchemaFlatten]
    public class Basket : DataStoreItem
    {
        [JsonSchemaType(typeof(List<ItemAndAmount>))]
        public List<ItemAndAmount> ProductAndQuantity { get; set; }
        public bool HasPlacedOrder { get; set; }

        public string UserUid { get; set; }

        public DateTime? DateOrdered { get; set; }
    }

    public class ItemAndAmount
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}