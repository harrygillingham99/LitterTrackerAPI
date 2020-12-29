using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace litter_tracker.Objects.Helpers
{
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
