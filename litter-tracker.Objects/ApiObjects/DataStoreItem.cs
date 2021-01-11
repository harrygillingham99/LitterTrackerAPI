namespace litter_tracker.Objects.ApiObjects
{
    /*
    Any object used in a Datastore repository must inherit from this type.
    */
    public abstract class DataStoreItem
    {
        public long DataStoreId { get; set; }
    }
}