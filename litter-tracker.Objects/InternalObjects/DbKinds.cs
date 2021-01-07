using System.ComponentModel;

namespace litter_tracker.Objects.InternalObjects
{
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
