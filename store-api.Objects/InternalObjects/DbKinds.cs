using System.ComponentModel;

namespace store_api.Objects.InternalObjects
{
    public static class DbKinds
    {
        public enum DbCollections
        {
            [Description("Categories")]
            Categories = 1,
            [Description("Products")]
            Products = 2,
            [Description("Baskets")]
            Baskets = 3,
            [Description("AdminUser")]
            AdminUser = 4

        }
    }
}
