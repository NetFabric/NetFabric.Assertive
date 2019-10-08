using System;
using System.Collections.Generic;

namespace NetFabric.Assertive
{
    static class EqualityComparer
    {
        public static void AssertEquality<TActual, TActualItem, TExpectedItem>(TActual actual, EnumerableInfo enumerableInfo, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
#if !NETSTANDARD2_1
            // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
            if (enumerableInfo.Current.PropertyType.IsByRef)
                return; // what should we do here?????
#endif

            if (!enumerableInfo.GetEnumerator.DeclaringType.IsInterface)
            {
                var actualItemType = enumerableInfo.Current.PropertyType;
                if (!typeof(TActualItem).IsAssignableFrom(actualItemType))
                    throw new AssertionException($"Expected {typeof(TActual)} to be an enumerable of {typeof(TActualItem)} but found an enumerable of {actualItemType}.");

                EqualityComparer.Compare(new EnumerableWrapper<TActualItem>(actual, enumerableInfo), expected, equalityComparison, enumerableInfo.GetEnumerator.DeclaringType);
            }

            foreach (var @interface in actual.GetType().GetInterfaces())
            {
                if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                {
                    var interfaceItemType = interfaceEnumerableInfo.Current.PropertyType;
                    if (!typeof(TActualItem).IsAssignableFrom(interfaceItemType))
                        throw new AssertionException($"Expected {typeof(TActual)} to be an enumerable of {typeof(TActualItem)} but found an enumerable of {interfaceItemType}.");

                    var wrapped = new EnumerableWrapper<TActualItem>(actual, interfaceEnumerableInfo);
                    var readOnlyCollectionType = typeof(IReadOnlyCollection<>).MakeGenericType(interfaceItemType);
                    var readOnlyListType = typeof(IReadOnlyList<>).MakeGenericType(interfaceItemType);

                    if (@interface == readOnlyCollectionType)
                    {
                        var actualCount = ((IReadOnlyCollection<TActualItem>)actual).Count;
                        var expectedCount = wrapped.Count();
                        if (actualCount != expectedCount)
                            throw new AssertionException($"Expected {actual.ToFriendlyString()} to have count value of {expectedCount} but found {actualCount}.");
                    } 
                    else if (@interface == readOnlyListType)
                    {
                        EqualityComparer.Compare((IReadOnlyList<TActualItem>)actual, expected, equalityComparison);
                    }
                    else
                    {
                        EqualityComparer.Compare(wrapped, expected, equalityComparison, @interface);
                    }
                }
            }
        }

        public static void Compare<TActualItem, TExpectedItem>(IEnumerable<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison, Type type)
        {
            using var actualEnumerator = actual.GetEnumerator();
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = !actualEnumerator.MoveNext();
                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return;

                    if (isActualCompleted ^ isExpectedCompleted)
                    {
                        if (isActualCompleted)
                            throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                                actual, 
                                expected,
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has less items when using {type}.GetEnumerator().");

                        if (isExpectedCompleted)
                            throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                                actual, 
                                expected,
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has more items when using {type}.GetEnumerator().");
                    }

                    if (!equalityComparison(actualEnumerator.Current, expectedEnumerator.Current))
                        throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                            actual, 
                            expected,
                            $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but if differs at index {index} when using {type}.GetEnumerator().");
                }
            }
        }

        public static void Compare<TActualItem, TExpectedItem>(IReadOnlyList<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = index == actual.Count;
                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return;

                    if (isActualCompleted ^ isExpectedCompleted)
                    {
                        if (isActualCompleted)
                            throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                                actual, 
                                expected,
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has less items when using the indexer.");

                        if (isExpectedCompleted)
                            throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                                actual, 
                                expected,
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has more items when using the indexer.");
                    }

                    if (!equalityComparison(actual[index], expectedEnumerator.Current))
                        throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                            actual, 
                            expected,
                            $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but if differs at index {index} when using the indexer.");
                }
            }
        }
    }
}