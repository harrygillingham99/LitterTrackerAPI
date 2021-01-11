using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace litter_tracker.Objects.Helpers
{
    /*
    Used to get the string value of a Description attribute.
    Used in this case to get the Kind from the DbKinds enum for the base class to use.
    */
    public static class DescriptionHelper
    {
        public static string GetDescription<T>(this T obj)
        {
            return obj.GetType()
                .GetMember(obj.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description ?? string.Empty;
        }
    }
}
