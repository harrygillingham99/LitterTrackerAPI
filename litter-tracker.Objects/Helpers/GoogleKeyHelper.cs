using System.Linq;
using Google.Cloud.Datastore.V1;
using store_api.Objects.InternalObjects;

namespace store_api.Objects.Helpers
{
    public static class GoogleKeyHelper
    {
        public static Key ToKey(this long id, DbKinds.DbCollections kind) =>
            new Key().WithElement(kind.GetDescription(), id);
        
        public static long ToId(this Key key) => key.Path.First().Id;
    }
}
