using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    //[DebuggerNonUserCode]
    public abstract class AssertionsBase
    {
        protected static void AssertIsEnumerable<TActual, TActualItem>(TActual actual, out EnumerableInfo enumerableInfo)
        {
            var actualType = typeof(TActual);
            if (actualType == typeof(TActualItem[])) // convert TActualItem[] to IList<TActualItem>
                actualType = typeof(IList<>).MakeGenericType(typeof(TActualItem));
            if (actualType.IsEnumerable(out var temp, out var errors))
            {
                enumerableInfo = temp;

                var actualItemType = enumerableInfo.EnumeratorInfo.Current.PropertyType;
                if (actualItemType.IsByRef)
                {
                    if (!actualItemType.IsAssignableTo(typeof(TActualItem).MakeByRefType()))
                        throw new ActualAssertionException<TActual>(actual, $"Expected to be an enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
                }
                else
                {
                    if (!actualItemType.IsAssignableTo(typeof(TActualItem)))
                        throw new ActualAssertionException<TActual>(actual, $"Expected to be an enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
                }
            }
            else
            {
                if (errors.HasFlag(Errors.MissingGetEnumerable))
                    throw new ActualAssertionException<TActual>(actual, $"Expected to be an enumerable but it's missing a valid 'GetEnumerator' method.");
                if (errors.HasFlag(Errors.MissingCurrent))
                    throw new ActualAssertionException<TActual>(actual, $"Expected to be an enumerator but it's missing a valid 'Current' property.");
                if (errors.HasFlag(Errors.MissingMoveNext))
                    throw new ActualAssertionException<TActual>(actual, $"Expected to be an enumerator but it's missing a valid 'MoveNext' method.");

                enumerableInfo = default!;
            }
        }

        protected static void AssertIsAsyncEnumerable<TActual, TActualItem>(TActual actual, out AsyncEnumerableInfo enumerableInfo)
        {
            var actualType = typeof(TActual);
            if (actualType.IsAsyncEnumerable(out var temp, out var errors))
            {
                enumerableInfo = temp;

                var actualItemType = enumerableInfo.EnumeratorInfo.Current.PropertyType;
                if (actualItemType.IsByRef)
                {
                    if (!actualItemType.IsAssignableTo(typeof(TActualItem).MakeByRefType()))
                        throw new ActualAssertionException<TActual>(actual, $"Expected to be an async enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
                }
                else
                {
                    if (!actualItemType.IsAssignableTo(typeof(TActualItem)))
                        throw new ActualAssertionException<TActual>(actual, $"Expected to be an async enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
                }
            }
            else
            {
                if (errors.HasFlag(Errors.MissingGetEnumerable))
                    throw new ActualAssertionException<TActual>(actual, $"Expected to be an async enumerable but it's missing a valid 'GetAsyncEnumerator' method.");
                if (errors.HasFlag(Errors.MissingCurrent))
                    throw new ActualAssertionException<TActual>(actual, $"Expected to be an async enumerator but it's missing a valid 'Current' property.");
                if (errors.HasFlag(Errors.MissingMoveNext))
                    throw new ActualAssertionException<TActual>(actual, $"Expected to be an async enumerator but it's missing a valid 'MoveNextAsync' method.");

                enumerableInfo = default!;
            }
        }

        protected static void AssertEnumerableEquality<TActual, TActualItem, TExpected, TExpectedItem>(
            [DisallowNull]TActual actual, EnumerableInfo actualEnumerableInfo, 
            [DisallowNull]TExpected expected, 
            Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
#if !NETSTANDARD2_1 // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
            if (actualEnumerableInfo.EnumeratorInfo.Current.PropertyType.IsByRef)
                return;
#endif

            var wrapped = new EnumerableWrapper<TActual, TActualItem>(actual, actualEnumerableInfo);
            switch (wrapped.Compare(expected, comparer, out var index))
            {
                case EqualityResult.NotEqualAtIndex:
                    throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                        wrapped,
                        expected,
                        $"Actual differs at index {index} when using '{actualEnumerableInfo.GetEnumerator.DeclaringType}.GetEnumerator()'.");

                case EqualityResult.LessItem:
                    throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                        wrapped,
                        expected,
                        $"Actual has less items when using '{actualEnumerableInfo.GetEnumerator.DeclaringType}.GetEnumerator()'.");

                case EqualityResult.MoreItems:
                    throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                        wrapped,
                        expected,
                        $"Actual has more items when using '{actualEnumerableInfo.GetEnumerator.DeclaringType}.GetEnumerator()'.");
            }
        }

        protected static void AssertDeepEnumerableEquality<TActual, TActualItem, TExpected, TExpectedItem>(
            [DisallowNull]TActual actual, 
            [DisallowNull]TExpected expected, 
            Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsEnumerable(out var enumerableInfo))
                {
                    var wrapped = new EnumerableWrapper<TActual, TActualItem>(actual, enumerableInfo);

#if !NETSTANDARD2_1 // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
                    if (enumerableInfo.EnumeratorInfo.Current.PropertyType.IsByRef)
                        continue;
#endif

                    switch (wrapped.Compare(expected, comparer, out var index))
                    {
                        case EqualityResult.NotEqualAtIndex:
                            throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                                wrapped,
                                expected,
                                $"Actual differs at index {index} when using '{@interface}.GetEnumerator()'.");

                        case EqualityResult.LessItem:
                            throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                                wrapped,
                                expected,
                                $"Actual has less items when using '{@interface}.GetEnumerator()'.");

                        case EqualityResult.MoreItems:
                            throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                                wrapped,
                                expected,
                                $"Actual has more items when using '{@interface}.GetEnumerator()'.");
                    }

                    var itemType = enumerableInfo.EnumeratorInfo.Current.PropertyType;
                    if (itemType.IsByRef)
                        itemType = itemType.GetElementType();

                    if (@interface.IsAssignableTo(typeof(IReadOnlyCollection<>).MakeGenericType(itemType)))
                    {
                        var actualCount = ((IReadOnlyCollection<TActualItem>)actual).Count;
                        var expectedCount = wrapped.Count();
                        if (actualCount != expectedCount)
                            throw new CountAssertionException(actualCount, expectedCount);
                    }

                    if (@interface.IsAssignableTo(typeof(IReadOnlyList<>).MakeGenericType(itemType)))
                    {
                        var readOnlyListActual = (IReadOnlyList<TActualItem>)actual;
                        switch (readOnlyListActual.Compare(expected, comparer, out index))
                        {
                            case EqualityResult.NotEqualAtIndex:
                                throw new ReadOnlyListAssertionException<TActualItem, TExpected>(
                                    new ReadOnlyListWrapper<TActualItem>(readOnlyListActual),
                                    expected,
                                    $"Actual differs at index {index} when using the indexer.");

                            case EqualityResult.LessItem:
                                throw new ReadOnlyListAssertionException<TActualItem, TExpected>(
                                    new ReadOnlyListWrapper<TActualItem>(readOnlyListActual),
                                    expected,
                                    $"Actual has less items when using the indexer.");

                            case EqualityResult.MoreItems:
                                throw new ReadOnlyListAssertionException<TActualItem, TExpected>(
                                    new ReadOnlyListWrapper<TActualItem>(readOnlyListActual),
                                    expected,
                                    $"Actual has more items when using the indexer.");
                        }
                    }
                }
            }
        }

        protected static void AssertAsyncEnumerableEquality<TActual, TActualItem, TExpected, TExpectedItem>(TActual actual, AsyncEnumerableInfo actualEnumerableInfo, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
#if !NETSTANDARD2_1 // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
            if (actualEnumerableInfo.EnumeratorInfo.Current.PropertyType.IsByRef)
                return;
#endif

            var getEnumeratorDeclaringType = actualEnumerableInfo.GetAsyncEnumerator.DeclaringType;
            var actualItemType = actualEnumerableInfo.EnumeratorInfo.Current.PropertyType;
            var wrapped = new AsyncEnumerableWrapper<TActual, TActualItem>(actual, actualEnumerableInfo);
            (var result, var index) = wrapped.Compare(expected, comparer).GetAwaiter().GetResult();
            switch (result)
            {
                case EqualityResult.NotEqualAtIndex:
                    {
                        throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                            wrapped,
                            expected,
                            $"Actual differs at index {index} when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                    }

                case EqualityResult.LessItem:
                    {
                        throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                            wrapped,
                            expected,
                            $"Actual has less items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                    }

                case EqualityResult.MoreItems:
                    {
                        throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                            wrapped,
                            expected,
                            $"Actual has more items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                    }
            }
        }

        protected static void AssertDeepAsyncEnumerableEquality<TActual, TActualItem, TExpected, TExpectedItem>(TActual actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsAsyncEnumerable(out var enumerableInfo))
                {
                    var wrapped = new AsyncEnumerableWrapper<TActual, TActualItem>(actual, enumerableInfo);

#if !NETSTANDARD2_1 // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
                    if (enumerableInfo.EnumeratorInfo.Current.PropertyType.IsByRef)
                        continue;
#endif
                    (var result, var index) = wrapped.Compare(expected, comparer).GetAwaiter().GetResult();
                    switch (result)
                    {
                        case EqualityResult.NotEqualAtIndex:
                            {
                                throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                                    wrapped,
                                    expected,
                                    $"Actual differs at index {index} when using '{@interface}.GetEnumerator()'.");
                            }

                        case EqualityResult.LessItem:
                            {
                                throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                                    wrapped,
                                    expected,
                                    $"Actual has less items when using '{@interface}.GetEnumerator()'.");
                            }

                        case EqualityResult.MoreItems:
                            {
                                throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                                    wrapped,
                                    expected,
                                    $"Actual has more items when using '{@interface}.GetEnumerator()'.");
                            }
                    }
                }
            }
        }

    }
}