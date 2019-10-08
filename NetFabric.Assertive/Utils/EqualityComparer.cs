using System;
using System.Collections.Generic;

namespace NetFabric.Assertive
{
    static class EqualityComparer
    {
        public static void AssertEquality<TActual, TActualItem, TExpected, TExpectedItem>(TActual actual, EnumerableInfo enumerableInfo, TExpected expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (!enumerableInfo.GetEnumerator.DeclaringType.IsInterface)
            {
                var actualItemType = enumerableInfo.Current.PropertyType;
                var wrapped = new EnumerableWrapper<TActualItem>(actual, enumerableInfo);

#if NETSTANDARD2_1 // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
                CompareEnumerable(actual, wrapped, expected, equalityComparison, enumerableInfo.GetEnumerator.DeclaringType);
#else
                if (enumerableInfo.Current.PropertyType.IsByRef)
                {
                    // what to do here?????
                }
                else
                {
                    CompareEnumerable(actual, wrapped, expected, equalityComparison, enumerableInfo.GetEnumerator.DeclaringType);
                }
#endif
            }

            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                {
                    var interfaceItemType = interfaceEnumerableInfo.Current.PropertyType;
                    var wrapped = new EnumerableWrapper<TActualItem>(actual, interfaceEnumerableInfo);
                    var readOnlyCollectionType = typeof(IReadOnlyCollection<>).MakeGenericType(interfaceItemType);
                    var readOnlyListType = typeof(IReadOnlyList<>).MakeGenericType(interfaceItemType);

                    CompareEnumerable(actual, wrapped, expected, equalityComparison, @interface);

                    if (@interface == readOnlyCollectionType)
                    {
                        var actualCount = ((IReadOnlyCollection<TActualItem>)actual).Count;
                        var expectedCount = wrapped.Count();
                        if (actualCount != expectedCount)
                            throw new EqualToAssertionException<TActual, TExpected>(
                                actual,
                                expected,
                                $"Expected '{actual.ToFriendlyString()}' to have count value of {expectedCount} but found {actualCount}.");
                    } 
                    
                    if (@interface == readOnlyListType)
                    {
                        CompareReadOnlyList(actual, expected, equalityComparison);
                    }
                }
            }
        }

        static void CompareEnumerable<TActual, TActualItem, TExpected, TExpectedItem>(TActual actual, IEnumerable<TActualItem> wrappedActual, TExpected expected, Func<TActualItem, TExpectedItem, bool> equalityComparison, Type type)
            where TExpected : IEnumerable<TExpectedItem>
        {
            using var actualEnumerator = wrappedActual.GetEnumerator();
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = !actualEnumerator.MoveNext();
                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return;

                    if (isActualCompleted)
                        throw new EqualToAssertionException<TActual, TExpected>(
                            actual, 
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{wrappedActual.ToFriendlyString()}' with less items when using '{type}.GetEnumerator()'.");

                    if (isExpectedCompleted)
                        throw new EqualToAssertionException<TActual, TExpected>(
                            actual, 
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{wrappedActual.ToFriendlyString()}' with more items when using '{type}.GetEnumerator()'.");

                    if (!equalityComparison(actualEnumerator.Current, expectedEnumerator.Current))
                        throw new EqualToAssertionException<TActual, TExpected>(
                            actual, 
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{wrappedActual.ToFriendlyString()}' that differs at index {index} when using '{type}.GetEnumerator()'.");
                }
            }
        }

        static void CompareReadOnlyList<TActual, TActualItem, TExpected, TExpectedItem>(TActual actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
            where TExpected : IEnumerable<TExpectedItem>
        {
            var actualReadOnlyList = (IReadOnlyList<TActualItem>)actual;

            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = false;
                    var actualItem = default(TActualItem);
                    try
                    {
                        actualItem = actualReadOnlyList[index];
                    }
                    catch
                    {
                        isActualCompleted = true;
                    }

                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return;

                    if (isActualCompleted)
                    {
                        var wrappedActual = new ReadOnlyListWrapper<TActualItem>(actualReadOnlyList);
                        throw new EqualToAssertionException<TActual, TExpected>(
                            actual,
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{wrappedActual.ToFriendlyString()}' with less items when using the indexer.");
                    }

                    if (isExpectedCompleted)
                    {
                        var wrappedActual = new ReadOnlyListWrapper<TActualItem>(actualReadOnlyList);
                        throw new EqualToAssertionException<TActual, TExpected>(
                            actual,
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{wrappedActual.ToFriendlyString()}' with more items when using the indexer.");
                    }

                    if (!equalityComparison(actualItem, expectedEnumerator.Current))
                    {
                        var wrappedActual = new ReadOnlyListWrapper<TActualItem>(actualReadOnlyList);
                        throw new EqualToAssertionException<TActual, TExpected>(
                            actual,
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{wrappedActual.ToFriendlyString()}' that differs at index {index} when using the indexer.");
                    }
                }
            }
        }
    }
}