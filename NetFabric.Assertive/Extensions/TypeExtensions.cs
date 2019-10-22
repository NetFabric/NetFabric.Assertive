using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class TypeExtensions
    {
        public static bool IsAssignableTo(this Type type, Type toType)
            => toType.IsAssignableFrom(type);

        public static bool IsEnumerable(this Type type, out EnumerableInfo info)
        {
            info = type.GetEnumerableInfo();
            return
                info.GetEnumerator is object &&
                info.Current is object &&
                info.MoveNext is object;
        }

        public static bool IsAsyncEnumerable(this Type type, out EnumerableInfo info)
        {
            info = type.GetAsyncEnumerableInfo();
            return
                info.GetEnumerator is object &&
                info.Current is object &&
                info.MoveNext is object;
        }

        public static EnumerableInfo GetEnumerableInfo(this Type type)
        {
            var getEnumerator = type.GetPublicOrExplicitMethod("GetEnumerator");
            if (getEnumerator is null)
                return default;

            var enumeratorType = getEnumerator.ReturnType;
            return new EnumerableInfo(
                getEnumerator,
                enumeratorType.GetPublicOrExplicitProperty("Current"),
                enumeratorType.GetPublicOrExplicitMethod("MoveNext"),
                enumeratorType.GetPublicOrExplicitMethod("Dispose"));
        }

        public static EnumerableInfo GetAsyncEnumerableInfo(this Type type)
        {
            var getEnumerator = type.GetPublicOrExplicitMethod("GetAsyncEnumerator");
            if (getEnumerator is null)
                getEnumerator = type.GetPublicOrExplicitMethod("GetAsyncEnumerator", typeof(CancellationToken));
            if (getEnumerator is null)
                return default;

            var enumeratorType = getEnumerator.ReturnType;
            return new EnumerableInfo(
                getEnumerator,
                enumeratorType.GetPublicOrExplicitProperty("Current"),
                enumeratorType.GetPublicOrExplicitMethod("MoveNextAsync"),
                enumeratorType.GetPublicOrExplicitMethod("DisposeAsync"));
        }

        static PropertyInfo GetPublicOrExplicitProperty(this Type type, string name)
        {
            var property = type.GetPublicProperty(name);
            if (property is object)
                return property;

            foreach (var @interface in type.GetInterfaces())
            {
                property = @interface.GetPublicProperty(name);
                if (property is object)
                    return property;
            }

            return null;
        }

        static MethodInfo GetPublicOrExplicitMethod(this Type type, string name, params Type[] parameters)
        {
            var method = type.GetPublicMethod(name, parameters);
            if (method is object)
                return method;

            foreach (var @interface in type.GetInterfaces())
            {
                method = @interface.GetPublicMethod(name, parameters);
                if (method is object)
                    return method;
            }

            return null;
        }

        const BindingFlags InstancePublicFlatten = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

        static PropertyInfo GetPublicProperty(this Type type, string name)
            => type.GetProperties(InstancePublicFlatten)
                .FirstOrDefault(property => property.Name == name && property.GetGetMethod() is object);

        static MethodInfo GetPublicMethod(this Type type, string name, params Type[] parameters)
            => type.GetMethods(InstancePublicFlatten)
                .FirstOrDefault(method => 
                    method.Name == name && 
                    parameters.SequenceEqual(method.GetParameters().Select(parameter => parameter.ParameterType)));
    }
}