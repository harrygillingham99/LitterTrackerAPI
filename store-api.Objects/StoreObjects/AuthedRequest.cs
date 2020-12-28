namespace store_api.Objects.StoreObjects
{
    public class AuthedRequest
    {
        public string JwtToken { get; set; }
    }

    public class AuthedBasketRequestWrapper
    {
        public Basket Basket { get; set; }
        public AuthedRequest Token { get; set; }
    }
}
