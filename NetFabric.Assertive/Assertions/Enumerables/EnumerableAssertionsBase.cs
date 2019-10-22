using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableAssertionsBase<TActual> 
        : AssertionsBase<TActual> 
    {
        internal EnumerableAssertionsBase(TActual Actual, EnumerableInfo enumerableInfo) 
            : base(Actual)
        {
            EnumerableInfo = enumerableInfo;
        }

        public EnumerableInfo EnumerableInfo { get; }


        protected void AssertEquality<TActualItem, TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
#if !NETSTANDARD2_1 // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
            if (EnumerableInfo.ItemType.IsByRef)
                return;
#endif

            var getEnumeratorDeclaringType = EnumerableInfo.GetEnumerator.DeclaringType;
            var wrapped = new EnumerableWrapper<TActual, TActualItem>(Actual, EnumerableInfo);
            switch (wrapped.Compare(expected, comparer, out var index))
            {
                case EqualityResult.NotEqualAtIndex:
                    throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                        wrapped,
                        expected,
                        $"Actual differs at index {index} when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");

                case EqualityResult.LessItem:
                    throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                        wrapped,
                        expected,
                        $"Actual has less items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");

                case EqualityResult.MoreItems:
                    throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                        wrapped,
                        expected,
                        $"Actual has more items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
            }
        }

        protected void AssertDeepEquality<TActualItem, TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                {
                    var wrappedInterface = new EnumerableWrapper<TActual, TActualItem>(Actual, interfaceEnumerableInfo);
                    switch (wrappedInterface.Compare(expected, comparer, out var interfaceIndex))
                    {
                        case EqualityResult.NotEqualAtIndex:
                            throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                                wrappedInterface,
                                expected,
                                $"Actual differs at index {interfaceIndex} when using '{@interface}.GetEnumerator()'.");

                        case EqualityResult.LessItem:
                            throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                                wrappedInterface,
                                expected,
                                $"Actual has less items when using '{@interface}.GetEnumerator()'.");

                        case EqualityResult.MoreItems:
                            throw new EnumerableAssertionException<TActual, TActualItem, TExpected>(
                                wrappedInterface,
                                expected,
                                $"Actual has more items when using '{@interface}.GetEnumerator()'.");
                    }

                    var interfaceItemType = interfaceEnumerableInfo.ItemType;
                    if (interfaceItemType.IsByRef)
                        interfaceItemType = interfaceItemType.GetElementType();

                    if (@interface.IsAssignableTo(typeof(IReadOnlyCollection<>).MakeGenericType(interfaceItemType)))
                    {
                        var actualCount = ((IReadOnlyCollection<TActualItem>)Actual).Count;
                        var expectedCount = wrappedInterface.Count();
                        if (actualCount != expectedCount)
                            throw new CountAssertionException(actualCount, expectedCount);
                    }

                    if (@interface.IsAssignableTo(typeof(IReadOnlyList<>).MakeGenericType(interfaceItemType)))
                    {
                        var readOnlyListActual = (IReadOnlyList<TActualItem>)Actual;
                        switch (readOnlyListActual.Compare(expected, comparer, out interfaceIndex))
                        {
                            case EqualityResult.NotEqualAtIndex:
                                throw new ReadOnlyListAssertionException<TActualItem, TExpected>(
                                    new ReadOnlyListWrapper<TActualItem>(readOnlyListActual),
                                    expected,
                                    $"Actual differs at index {interfaceIndex} when using the indexer.");

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
    }
}