using System;

namespace store_api.Objects.StoreObjects
{
    public class Product : DataStoreItem
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal PricePerUnit { get; set; }
        public DateTime DateCreated { get; set; }

    }
}