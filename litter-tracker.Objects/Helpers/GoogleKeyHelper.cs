using System.Linq;
using Google.Cloud.Datastore.V1;
using litter_tracker.Objects.InternalObjects;

namespace litter_tracker.Objects.Helpers
{
    /*
    Helper class used to transform a data store id into a Google Datasore Key object and visa versa.
    */
    public static class GoogleKeyHelper
    {
        public static Key ToKey(this long id, DbKinds.DbCollections kind) =>
            new Key().WithElement(kind.GetDescription(), id);
        
        public static long ToId(this Key key) => key.Path.First().Id;
    }
}
