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
    }
}
