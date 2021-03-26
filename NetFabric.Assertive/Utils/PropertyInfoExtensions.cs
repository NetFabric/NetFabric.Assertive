using System.Reflection;

namespace NetFabric.Assertive
{
    static class PropertyInfoExtensions
    {
        public static bool IsByRef(this PropertyInfo info)
            => info.PropertyType.IsByRef;
    }
}