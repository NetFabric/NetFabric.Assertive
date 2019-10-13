using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static partial class TypeExtensions
    {
        public static bool IsEnumerable(this Type type, out EnumerableInfo info)
        {
            var getEnumerator = type.GetPublicOrExplicitMethod("GetEnumerator");
            if (getEnumerator is null)
            {
                info = default;
                return false;
            }
    
            var enumeratorType = getEnumerator.ReturnType;
            info = new EnumerableInfo(getEnumerator,
                enumeratorType.GetPublicOrExplicitProperty("Current"),
                enumeratorType.GetPublicOrExplicitMethod("MoveNext"),
                enumeratorType.GetPublicOrExplicitMethod("Dispose"));
            return true;
        }

        public static void AssertIsEnumerable<TActual, TActualItem>(TActual actual, out EnumerableInfo enumerableInfo)
        {
            var actualType = typeof(TActual);
            enumerableInfo = actualType.GetEnumerableInfo();

            if (enumerableInfo.GetEnumerator is null)
                throw new ActualAssertionException<TActual>(actual, $"Expected '{actualType}' to be an enumerable but it's missing a valid 'GetEnumerator' method.");
            if (enumerableInfo.Current is null)
                throw new ActualAssertionException<TActual>(actual, $"Expected '{enumerableInfo.GetEnumerator.ReturnType}' to be an enumerator but it's missing a valid 'Current' property.");
            if (enumerableInfo.MoveNext is null)
                throw new ActualAssertionException<TActual>(actual, $"Expected '{enumerableInfo.GetEnumerator.ReturnType}' to be an enumerator but it's missing a valid 'MoveNext' method.");

            var actualItemType = enumerableInfo.Current.PropertyType;
            if (actualItemType.IsByRef)
            {
                if (!typeof(TActualItem).MakeByRefType().IsAssignableFrom(actualItemType))
                    throw new ActualAssertionException<TActual>(actual, $"Expected '{actualType}' to be an enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
            }
            else
            {
                if (!typeof(TActualItem).IsAssignableFrom(actualItemType))
                    throw new ActualAssertionException<TActual>(actual, $"Expected '{actualType}' to be an enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
            }
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

        public static MethodInfo GetPublicOrExplicitMethod(this Type type, string name, params Type[] parameters)
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