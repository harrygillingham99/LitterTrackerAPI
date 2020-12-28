using System;
using System.Collections.Generic;
using System.Text;
using store_api.Objects.StoreObjects;

namespace store_api.Objects.Helpers
{
    public static class ProductHelper
    {
        public static Product EnsureCreatedDate(this Product product)
        {
            product.DateCreated = DateTime.Now;
            return product;
        }
    }
}
