using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public abstract class AssertionsBase<TAssertions>
        where TAssertions : AssertionsBase<TAssertions>
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
            TActual actual, EnumerableInfo actualEnumerableInfo, 
            TExpected expected, 
            Func<TActualItem, TExpectedItem, bool> comparer,
            bool testNonGeneric,
            bool testIndexOf,
            IEnumerable<TActualItem>? doesNotContain)
            where TActual : notnull
            where TExpected : IEnumerable<TExpectedItem>
        {
            var wrapped = new EnumerableWrapper<TActual, TActualItem>(actual, actualEnumerableInfo);
            var (result, index, _, _) = actual.Compare(expected, comparer);
            switch (result)
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

            if (typeof(TActual).IsAssignableTo(typeof(IReadOnlyCollection<>).MakeGenericType(typeof(TActualItem))))
            {
                // test Count
                var actualCount = ((IReadOnlyCollection<TActualItem>)actual).Count;
                var expectedCount = wrapped.Count();
                if (actualCount != expectedCount)
                    throw new CountAssertionException(actualCount, expectedCount);
            }

            var publicCount = typeof(TActual).GetProperty("Count", BindingFlags.Public | BindingFlags.Instance, null, typeof(int), Type.EmptyTypes, null);
            var publicIndexer = typeof(TActual).GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, typeof(TActualItem), new[] { typeof(int) }, null);
            if (publicCount is not null && publicIndexer is not null)
            {
                var wrappedActual = new IndexerWrapper<TActual, TActualItem>(actual, publicIndexer);
                (result, index, _, _) = wrappedActual.Compare(expected, comparer);
                switch (result)
                {
                    case EqualityResult.NotEqualAtIndex:
                        throw new EqualToAssertionException<IndexerWrapper<TActual, TActualItem>, TExpected>(
                            wrappedActual,
                            expected,
                            $"Actual differs at index {index} when using the indexer {typeof(TActual)}.Item[System.Int32].");

                    case EqualityResult.LessItem:
                        throw new EqualToAssertionException<IndexerWrapper<TActual, TActualItem>, TExpected>(
                            wrappedActual,
                            expected,
                            $"Actual has less items when using the indexer {typeof(TActual)}.Item[System.Int32].");

                    case EqualityResult.MoreItems:
                        throw new EqualToAssertionException<IndexerWrapper<TActual, TActualItem>, TExpected>(
                            wrappedActual,
                            expected,
                            $"Actual has more items when using the indexer {typeof(TActual)}.Item[System.Int32].");
                }
            }

            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsEnumerable(out var enumerableInfo))
                {
                    // test enumeration
                    var itemType = enumerableInfo.EnumeratorInfo.Current.PropertyType;
                    if (itemType.IsByRef)
                    {
                        itemType = itemType.GetElementType();
                    }

                    var isNonGeneric = itemType == typeof(object);
                    if (!testNonGeneric && isNonGeneric)
                        continue;

                    wrapped = new EnumerableWrapper<TActual, TActualItem>(actual, enumerableInfo);
                    (result, index, _, _) = wrapped.Compare(expected, comparer);
                    switch (result)
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
                }
            }

            if (typeof(TActual).IsAssignableTo(typeof(ICollection<>).MakeGenericType(typeof(TActualItem))))
            {
                var collectionActual = (ICollection<TActualItem>)actual;

                // test Contains
                try
                {
                    using var enumerator = collectionActual.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (!collectionActual.Contains(enumerator.Current))
                            throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                                new EnumerableWrapper<TActual, TActualItem>(actual, actualEnumerableInfo),
                                expected,
                                $"'Contains' return false for an item found when using 'System.Collections.Generic.IEnumerable`1[{typeof(TActualItem)}].GetEnumerator()'.");
                    }

                    if (doesNotContain is not null)
                    {
                        foreach (var item in doesNotContain)
                        {
                            if (collectionActual.Contains(item))
                            {
                                throw new ActualAssertionException<TActualItem>(
                                    item,
                                    $"'Contains' return true for the item '{item}'.");
                            }
                        }
                    }
                }
                catch (NotSupportedException)
                {
                    // do nothing
                }

                // test CopyTo
                try
                {
                    var wrappedActual = new CopyToWrapper<TActualItem>((ICollection<TActualItem>)actual, 10);
                    (result, index, _, _) = wrappedActual.Compare(expected, comparer);
                    switch (result)
                    {
                        case EqualityResult.NotEqualAtIndex:
                            throw new EqualToAssertionException<CopyToWrapper<TActualItem>, TExpected>(
                                wrappedActual,
                                expected,
                                $"Actual differs at index {index} when using the CopyTo.");

                        case EqualityResult.LessItem:
                            throw new EqualToAssertionException<CopyToWrapper<TActualItem>, TExpected>(
                                wrappedActual,
                                expected,
                                $"Actual has less items when using the CopyTo.");

                        case EqualityResult.MoreItems:
                            throw new EqualToAssertionException<CopyToWrapper<TActualItem>, TExpected>(
                                wrappedActual,
                                expected,
                                $"Actual has more items when using the CopyTo.");
                    }
                }
                catch (NotSupportedException)
                {
                    // do nothing
                }
            }

            if (typeof(TActual).IsAssignableTo(typeof(IReadOnlyList<>).MakeGenericType(typeof(TActualItem))))
            {
                // test the indexer
                try
                {
                    var wrappedActual = new ReadOnlyListWrapper<TActualItem>((IReadOnlyList<TActualItem>)actual);
                    (result, index, _, _) = wrappedActual.Compare(expected, comparer);
                    switch (result)
                    {
                        case EqualityResult.NotEqualAtIndex:
                            throw new EqualToAssertionException<ReadOnlyListWrapper<TActualItem>, TExpected>(
                                wrappedActual,
                                expected,
                                $"Actual differs at index {index} when using the indexer IReadOnlyList`1[{typeof(TActualItem)}].Item[System.Int32].");

                        case EqualityResult.LessItem:
                            throw new EqualToAssertionException<ReadOnlyListWrapper<TActualItem>, TExpected>(
                                wrappedActual,
                                expected,
                                $"Actual has less items when using the indexer IReadOnlyList`1[{typeof(TActualItem)}].Item[System.Int32].");

                        case EqualityResult.MoreItems:
                            throw new EqualToAssertionException<ReadOnlyListWrapper<TActualItem>, TExpected>(
                                wrappedActual,
                                expected,
                                $"Actual has more items when using the indexer IReadOnlyList`1[{typeof(TActualItem)}].Item[System.Int32].");
                    }
                }
                catch (NotSupportedException)
                {
                    // do nothing
                }
            }

            if (typeof(TActual).IsAssignableTo(typeof(IList<>).MakeGenericType(typeof(TActualItem))))
            {
                var listActual = (IList<TActualItem>)actual;
                var wrappedActual = new ListWrapper<IList<TActualItem>, TActualItem>(listActual);

                // test the indexer
                try
                {
                    (result, index, _, _) = wrappedActual.Compare(expected, comparer);
                    switch (result)
                    {
                        case EqualityResult.NotEqualAtIndex:
                            throw new EqualToAssertionException<ListWrapper<IList<TActualItem>, TActualItem>, TExpected>(
                                wrappedActual,
                                expected,
                                $"Actual differs at index {index} when using the indexer IList`1[{typeof(TActualItem)}].Item[System.Int32].");

                        case EqualityResult.LessItem:
                            throw new EqualToAssertionException<ListWrapper<IList<TActualItem>, TActualItem>, TExpected>(
                                wrappedActual,
                                expected,
                                $"Actual has less items when using the indexer IList`1[{typeof(TActualItem)}].Item[System.Int32].");

                        case EqualityResult.MoreItems:
                            throw new EqualToAssertionException<ListWrapper<IList<TActualItem>, TActualItem>, TExpected>(
                                wrappedActual,
                                expected,
                                $"Actual has more items when using the indexer IList`1[{typeof(TActualItem)}].Item[System.Int32].");
                    }
                }
                catch (NotSupportedException)
                {
                    // do nothing
                }

                // test IndexOf
                if (testIndexOf)
                {
                    for (index = 0; index < listActual.Count; index++)
                    {
                        var item = listActual[index];
                        var itemIndex = 0;
                        
                        try
                        {
                            itemIndex = listActual.IndexOf(item);
                        }
                        catch (Exception exception)
                        {
                            throw new AssertionException("Unhandled exception in IList`1.IndexOf().", exception);
                        }

                        if (itemIndex != index)
                        {
                            throw new EqualToAssertionException<int, int>(
                                itemIndex,
                                index,
                                $"Actual differs at index {index} when using IList`1.IndexOf({item}).");
                        }
                    }
                }

                if (doesNotContain is not null)
                {
                    try
                    {
                        foreach (var item in doesNotContain)
                        {
                            index = listActual.IndexOf(item);
                            if (index >= 0)
                            {
                                throw new ActualAssertionException<int>(
                                    index,
                                    $"'IndexOf' return '{index}' for the item '{item}'.");
                            }
                        }
                    }
                    catch (NotSupportedException)
                    {
                        // do nothing
                    }
                }
            }
        }

        protected static void AssertAsyncEnumerableEquality<TActual, TActualItem, TExpected, TExpectedItem>(
            TActual actual, 
            AsyncEnumerableInfo actualEnumerableInfo, TExpected expected, 
            Func<TActualItem, TExpectedItem, bool> comparer, 
            bool testNonGeneric = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            var getEnumeratorDeclaringType = actualEnumerableInfo.GetAsyncEnumerator.DeclaringType;
            var actualItemType = actualEnumerableInfo.EnumeratorInfo.Current.PropertyType;
            var wrapped = new AsyncEnumerableWrapper<TActual, TActualItem>(actual, actualEnumerableInfo);
            var (result, index, _, _) = wrapped.CompareAsync(expected, comparer).GetAwaiter().GetResult();
            switch (result)
            {
                case EqualityResult.NotEqualAtIndex:
                    throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                        wrapped,
                        expected,
                        $"Actual differs at index {index} when using '{getEnumeratorDeclaringType}.GetAsyncEnumerator()'.");

                case EqualityResult.LessItem:
                    throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                        wrapped,
                        expected,
                        $"Actual has less items when using '{getEnumeratorDeclaringType}.GetAsyncEnumerator()'.");

                case EqualityResult.MoreItems:
                    throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                        wrapped,
                        expected,
                        $"Actual has more items when using '{getEnumeratorDeclaringType}.GetAsyncEnumerator()'.");
            }

            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsAsyncEnumerable(out var enumerableInfo))
                {
                    // test enumeration
                    var itemType = enumerableInfo.EnumeratorInfo.Current.PropertyType;
                    if (itemType.IsByRef)
                    {
                        itemType = itemType.GetElementType();
                    }

                    var isNonGeneric = itemType == typeof(object);
                    if (!testNonGeneric && isNonGeneric)
                        continue;

                    wrapped = new AsyncEnumerableWrapper<TActual, TActualItem>(actual, enumerableInfo);
                    (result, index, _, _) = wrapped.CompareAsync(expected, comparer).GetAwaiter().GetResult();
                    switch (result)
                    {
                        case EqualityResult.NotEqualAtIndex:
                            {
                                throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                                    wrapped,
                                    expected,
                                    $"Actual differs at index {index} when using '{@interface}.GetAsyncEnumerator()'.");
                            }

                        case EqualityResult.LessItem:
                            {
                                throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                                    wrapped,
                                    expected,
                                    $"Actual has less items when using '{@interface}.GetAsyncEnumerator()'.");
                            }

                        case EqualityResult.MoreItems:
                            {
                                throw new AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>(
                                    wrapped,
                                    expected,
                                    $"Actual has more items when using '{@interface}.GetAsyncEnumerator()'.");
                            }
                    }
                }
            }
        }

    }
}