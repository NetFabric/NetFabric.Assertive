using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    static partial class TypeExtensions
    {
        public static bool IsAsyncEnumerable(this Type type, out EnumerableInfo info)
        {
            var getEnumerator = type.GetPublicOrExplicitMethod("GetAsyncEnumerator");
            if (getEnumerator is null)
            {
                info = default;
                return false;
            }

            var enumeratorType = getEnumerator.ReturnType;
            info = new EnumerableInfo(getEnumerator,
                enumeratorType.GetPublicOrExplicitProperty("Current"),
                enumeratorType.GetPublicOrExplicitMethod("MoveNextAsync"),
                enumeratorType.GetPublicOrExplicitMethod("DisposeAsync"));
            return true;
        }

        public static void AssertIsAsyncEnumerable<TActual, TActualItem>(TActual actual, out EnumerableInfo enumerableInfo)
        {
            var actualType = typeof(TActual);
            enumerableInfo = actualType.GetAsyncEnumerableInfo();

            if (enumerableInfo.GetEnumerator is null)
                throw new ActualAssertionException<TActual>(actual, $"Expected '{actualType}' to be an async enumerable but it's missing a valid 'GetAsyncEnumerator' method.");
            if (enumerableInfo.Current is null)
                throw new ActualAssertionException<TActual>(actual, $"Expected '{enumerableInfo.GetEnumerator.ReturnType}' to be an async enumerator but it's missing a valid 'Current' property.");
            if (enumerableInfo.MoveNext is null)
                throw new ActualAssertionException<TActual>(actual, $"Expected '{enumerableInfo.GetEnumerator.ReturnType}' to be an async enumerator but it's missing a valid 'MoveNextAsync' method.");

            var actualItemType = enumerableInfo.Current.PropertyType;
            if (actualItemType.IsByRef)
            {
                if (!typeof(TActualItem).MakeByRefType().IsAssignableFrom(actualItemType))
                    throw new ActualAssertionException<TActual>(actual, $"Expected '{actualType}' to be an async enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
            }
            else
            {
                if (!typeof(TActualItem).IsAssignableFrom(actualItemType))
                    throw new ActualAssertionException<TActual>(actual, $"Expected '{actualType}' to be an async enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
            }
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
    }
}