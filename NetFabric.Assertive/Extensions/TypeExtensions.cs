using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class TypeExtensions
    {
        public static bool  IsEnumerable(this Type type, out EnumerableInfo info)
        {
            var getEnumerator = type.GetPublicOrExplicitParameterlessMethod("GetEnumerator");
            if (getEnumerator is null)
            {
                info = default;
                return false;
            }
    
            var enumeratorType = getEnumerator.ReturnType;
            info = new EnumerableInfo(getEnumerator,
                enumeratorType.GetPublicOrExplicitProperty("Current"),
                enumeratorType.GetPublicOrExplicitParameterlessMethod("MoveNext"),
                enumeratorType.GetPublicOrExplicitParameterlessMethod("Dispose"));
            return true;
        }

        public static EnumerableInfo GetEnumerableInfo(this Type type)
        {
            var getEnumerator = type.GetPublicOrExplicitParameterlessMethod("GetEnumerator");
            if (getEnumerator is null)
                return default;

            var enumeratorType = getEnumerator.ReturnType;
            return new EnumerableInfo(
                getEnumerator,
                enumeratorType.GetPublicOrExplicitProperty("Current"),
                enumeratorType.GetPublicOrExplicitParameterlessMethod("MoveNext"),
                enumeratorType.GetPublicOrExplicitParameterlessMethod("Dispose"));
        }

        public static PropertyInfo GetPublicOrExplicitProperty(this Type type, string name)
        {
            var method = type.GetPublicProperty(name);
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicProperty(name);
                if (method is object)
                    return method;
            }

            return null;
        }

        public static MethodInfo GetPublicOrExplicitParameterlessMethod(this Type type, string name)
        {
            var method = type.GetPublicParameterlessMethod(name);
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicParameterlessMethod(name);
                if (method is object)
                    return method;
            }

            return null;
        }

        const BindingFlags InstancePublicFlatten = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        static PropertyInfo GetPublicProperty(this Type type, string name)
            => type.GetProperties(InstancePublicFlatten)
                .FirstOrDefault(property => property.Name == name && property.GetGetMethod() is object);

        static MethodInfo GetPublicParameterlessMethod(this Type type, string name)
            => type.GetMethods(InstancePublicFlatten)
                .FirstOrDefault(method => method.Name == name && method.GetParameters().Length == 0);
    }
}