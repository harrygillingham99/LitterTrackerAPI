using System.ComponentModel;

namespace litter_tracker.Objects.InternalObjects
{
    /*
    Enums used to store operation types and Datastore kinds.
    */
    public static class DbKinds
    {
        public enum DbCollections
        {
            [Description("LitterPin")]
            LitterPin = 1

        }

        public enum PinOperationType
        {
            CreatePin = 1,
            UpdatePin = 2
        }
    }
}
