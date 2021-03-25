using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class TypeExtensions
    {
        public static bool IsAssignableTo(this Type type, Type toType)
            => toType.IsAssignableFrom(type);

        public static bool IsRefStruct(this Type type)
            => type
                .GetCustomAttributes() // this works on target platform that don't support IsByRefLikeAttribute
                .FirstOrDefault(attribute => attribute.GetType().Name is "IsByRefLikeAttribute") is not null;
    }
}