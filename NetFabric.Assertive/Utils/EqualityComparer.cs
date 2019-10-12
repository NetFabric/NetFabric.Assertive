using System;
using System.Collections.Generic;

namespace NetFabric.Assertive
{
    static class EqualityComparer
    {
        public static void AssertEquality<TActual, TActualItem, TExpected, TExpectedItem>(TActual actual, EnumerableInfo enumerableInfo, TExpected expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
            where TExpected : IEnumerable<TExpectedItem>
        {
            var getEnumeratorDeclaringType = enumerableInfo.GetEnumerator.DeclaringType;
            if (!getEnumeratorDeclaringType.IsInterface)
            {
                var actualItemType = enumerableInfo.Current.PropertyType;
                var wrapped = new EnumerableWrapper<TActualItem>(actual, enumerableInfo);

#if NETSTANDARD2_1 
                // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
                if (enumerableInfo.Current.PropertyType.IsByRef)
                {
                    // what to do here?????
                }
                else
                {
#endif
                    (var result, var index) = wrapped.Compare(expected, equalityComparison);
                    switch (result)
                    {
                        case EqualityResult.NotEqualAtIndex:
                            {
                                throw new EqualToAssertionException<TActual, TExpected>(
                                    actual,
                                    expected,
                                    $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' that differs at index {index} when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                            }

                        case EqualityResult.LessItem:
                            {
                                throw new EqualToAssertionException<TActual, TExpected>(
                                    actual,
                                    expected,
                                    $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with less items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                            }

                        case EqualityResult.MoreItems:
                            {
                                throw new EqualToAssertionException<TActual, TExpected>(
                                    actual,
                                    expected,
                                    $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with more items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                            }
                    }
#if NETSTANDARD2_1 
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

                    (var result, var index) = wrapped.Compare(expected, equalityComparison);
                    switch (result)
                    {
                        case EqualityResult.NotEqualAtIndex:
                            {
                                throw new EqualToAssertionException<TActual, TExpected>(
                                    actual,
                                    expected,
                                    $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' that differs at index {index} when using '{@interface}.GetEnumerator()'.");
                            }

                        case EqualityResult.LessItem:
                            {
                                throw new EqualToAssertionException<TActual, TExpected>(
                                    actual,
                                    expected,
                                    $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with less items when using '{@interface}.GetEnumerator()'.");
                            }

                        case EqualityResult.MoreItems:
                            {
                                throw new EqualToAssertionException<TActual, TExpected>(
                                    actual,
                                    expected,
                                    $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with more items when using '{@interface}.GetEnumerator()'.");
                            }
                    }

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
                        var readOnlyListActual = (IReadOnlyList<TActualItem>)actual;
                        (result, index) = readOnlyListActual.Compare(expected, equalityComparison);
                        switch (result)
                        {
                            case EqualityResult.NotEqualAtIndex:
                                {
                                    var wrappedActual = new ReadOnlyListWrapper<TActualItem>(readOnlyListActual);
                                    throw new EqualToAssertionException<TActual, TExpected>(
                                        actual,
                                        expected,
                                        $"Expected '{expected.ToFriendlyString()}' but found '{wrappedActual.ToFriendlyString()}' that differs at index {index} when using the indexer.");
                                }

                            case EqualityResult.LessItem:
                                {
                                    var wrappedActual = new ReadOnlyListWrapper<TActualItem>(readOnlyListActual);
                                    throw new EqualToAssertionException<TActual, TExpected>(
                                        actual,
                                        expected,
                                        $"Expected '{expected.ToFriendlyString()}' but found '{wrappedActual.ToFriendlyString()}' with less items when using the indexer.");
                                }

                            case EqualityResult.MoreItems:
                                {
                                    var wrappedActual = new ReadOnlyListWrapper<TActualItem>(readOnlyListActual);
                                    throw new EqualToAssertionException<TActual, TExpected>(
                                        actual,
                                        expected,
                                        $"Expected '{expected.ToFriendlyString()}' but found '{wrappedActual.ToFriendlyString()}' with more items when using the indexer.");
                                }
                        }
                    }
                }
            }
        }

        public static void AssertNotSharing<TActual, TActualItem>(TActual actual, EnumerableInfo enumerableInfo)
        {
            var getEnumeratorDeclaringType = enumerableInfo.GetEnumerator.DeclaringType;
            if (!getEnumeratorDeclaringType.IsInterface)
            {
                var actualItemType = enumerableInfo.Current.PropertyType;
                var wrapped = new EnumerableWrapper<TActualItem>(actual, enumerableInfo);

#if NETSTANDARD2_1
                // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
                if (enumerableInfo.Current.PropertyType.IsByRef)
                {
                    // what to do here?????
                }
                else
                {
#endif
                    (var result, var _) = wrapped.Compare(wrapped, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));
                    if (result != EqualityResult.Equal)
                        throw new ActualAssertionException<TActual>(actual, $"Enumerators returned by '{getEnumeratorDeclaringType}.GetEnumerator()' do share state.");
#if NETSTANDARD2_1
                }
#endif
            }

            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                {
                    var wrapped = new EnumerableWrapper<TActualItem>(actual, interfaceEnumerableInfo);

                    (var result, var _) = wrapped.Compare(wrapped, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));
                    if (result != EqualityResult.Equal)
                        throw new ActualAssertionException<TActual>(actual, $"Enumerators returned by '{@interface}.GetEnumerator()' do share state.");
                }
            }
        }

        enum EqualityResult
        {
            Equal,
            NotEqualAtIndex,
            LessItem,
            MoreItems,
        }

        static (EqualityResult Result, int Index) Compare<TActualItem, TExpectedItem>(this IEnumerable<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
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
                        return (EqualityResult.Equal, index);

                    if (isActualCompleted)
                        return (EqualityResult.LessItem, index);

                    if (isExpectedCompleted)
                        return (EqualityResult.MoreItems, index);

                    if (!equalityComparison(actualEnumerator.Current, expectedEnumerator.Current))
                        return (EqualityResult.NotEqualAtIndex, index);
                }
            }
        }

        static (EqualityResult Result, int Index) Compare<TActualItem, TExpectedItem>(this IReadOnlyList<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = false;
                    var actualItem = default(TActualItem);
                    try
                    {
                        actualItem = actual[index];
                    }
                    catch
                    {
                        isActualCompleted = true;
                    }

                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return (EqualityResult.Equal, index);

                    if (isActualCompleted)
                        return (EqualityResult.LessItem, index);

                    if (isExpectedCompleted)
                        return (EqualityResult.MoreItems, index);

                    if (!equalityComparison(actualItem, expectedEnumerator.Current))
                        return (EqualityResult.NotEqualAtIndex, index);
                }
            }
        }
    }
}