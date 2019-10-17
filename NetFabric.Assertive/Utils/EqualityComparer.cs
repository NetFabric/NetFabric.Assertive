using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Assertive
{
    static partial class EqualityComparer
    {
        public static void AssertEquality<TActual, TActualItem, TExpected, TExpectedItem>(TActual actual, EnumerableInfo enumerableInfo, TExpected expected)
            where TExpected : IEnumerable<TExpectedItem>
        {
#if !NETSTANDARD2_1
            // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
            if (enumerableInfo.Current.PropertyType.IsByRef)
                return; 
#endif

            var getEnumeratorDeclaringType = enumerableInfo.GetEnumerator.DeclaringType;
            if (!getEnumeratorDeclaringType.IsInterface)
            {
                var wrapped = new EnumerableWrapper<TActual>(actual, enumerableInfo);
                (var result, var index) = wrapped.Compare(expected);
                switch (result)
                {
                    case EqualityResult.NotEqualAtIndex:
                        throw new EqualToAssertionException<TActual, TExpected>(
                            actual,
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' that differs at index {index} when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");

                    case EqualityResult.LessItem:
                        throw new EqualToAssertionException<TActual, TExpected>(
                            actual,
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with less items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");

                    case EqualityResult.MoreItems:
                        throw new EqualToAssertionException<TActual, TExpected>(
                            actual,
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with more items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                }
            }

            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                {
                    var interfaceItemType = interfaceEnumerableInfo.Current.PropertyType;
                    var wrapped = new EnumerableWrapper<TActual>(actual, interfaceEnumerableInfo);
                    var readOnlyCollectionType = typeof(IReadOnlyCollection<>).MakeGenericType(interfaceItemType);
                    var readOnlyListType = typeof(IReadOnlyList<>).MakeGenericType(interfaceItemType);

                    (var result, var index) = wrapped.Compare(expected);
                    switch (result)
                    {
                        case EqualityResult.NotEqualAtIndex:
                            throw new EqualToAssertionException<TActual, TExpected>(
                                actual,
                                expected,
                                $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' that differs at index {index} when using '{@interface}.GetEnumerator()'.");

                        case EqualityResult.LessItem:
                            throw new EqualToAssertionException<TActual, TExpected>(
                                actual,
                                expected,
                                $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with less items when using '{@interface}.GetEnumerator()'.");

                        case EqualityResult.MoreItems:
                            throw new EqualToAssertionException<TActual, TExpected>(
                                actual,
                                expected,
                                $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with more items when using '{@interface}.GetEnumerator()'.");
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
                        (result, index) = readOnlyListActual.Compare(expected);
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
#if !NETSTANDARD2_1
            // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
            if (enumerableInfo.Current.PropertyType.IsByRef)
                return;
#endif

            var getEnumeratorDeclaringType = enumerableInfo.GetEnumerator.DeclaringType;
            if (!getEnumeratorDeclaringType.IsInterface)
            {
                var wrapped = new EnumerableWrapper<TActual>(actual, enumerableInfo);
                (var result, var _) = wrapped.Compare(wrapped);
                if (result != EqualityResult.Equal)
                    throw new ActualAssertionException<TActual>(actual, $"Enumerators returned by '{getEnumeratorDeclaringType}.GetEnumerator()' do share state.");
            }

            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                {
                    var wrapped = new EnumerableWrapper<TActual>(actual, interfaceEnumerableInfo);
                    (var result, var _) = wrapped.Compare(wrapped);
                    if (result != EqualityResult.Equal)
                        throw new ActualAssertionException<TActual>(actual, $"Enumerators returned by '{@interface}.GetEnumerator()' do share state.");
                }
            }
        }

        static (EqualityResult Result, int Index) Compare(this IEnumerable actual, IEnumerable expected)
        {
            var actualEnumerator = actual.GetEnumerator();
            var expectedEnumerator = expected.GetEnumerator();
            try
            {
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

                        if (!actualEnumerator.Current.Equals(expectedEnumerator.Current))
                            return (EqualityResult.NotEqualAtIndex, index);
                    }
                }
            }
            finally
            {
                if (actualEnumerator is IDisposable actualDisposable)
                    actualDisposable.Dispose();

                if (expectedEnumerator is IDisposable expectedDisposable)
                    expectedDisposable.Dispose();
            }
        }

        static (EqualityResult Result, int Index) Compare<TActualItem, TExpectedItem>(this IEnumerable<TActualItem> actual, IEnumerable<TExpectedItem> expected)
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

                    if (!actualEnumerator.Current.Equals(expectedEnumerator.Current))
                        return (EqualityResult.NotEqualAtIndex, index);
                }
            }
        }

        static (EqualityResult Result, int Index) Compare<TActualItem, TExpectedItem>(this IReadOnlyList<TActualItem> actual, IEnumerable<TExpectedItem> expected)
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

                    if (!actualItem.Equals(expectedEnumerator.Current))
                        return (EqualityResult.NotEqualAtIndex, index);
                }
            }
        }

        public static (EqualityResult Result, int Index) Compare<TActualItem, TExpectedItem>(this TActualItem[] actual, TExpectedItem[] expected, Func<TActualItem, TExpectedItem, bool> comparer)
        {
            if (actual.Length < expected.Length)
                return (EqualityResult.LessItem, actual.Length);

            if (actual.Length > expected.Length)
                return (EqualityResult.MoreItems, expected.Length);

            for (var index = 0; index < actual.Length; index++)
            {
                if (!comparer(actual[index], expected[index]))
                    return (EqualityResult.NotEqualAtIndex, index);
            }

            return (EqualityResult.Equal, actual.Length);
        }
    }
}