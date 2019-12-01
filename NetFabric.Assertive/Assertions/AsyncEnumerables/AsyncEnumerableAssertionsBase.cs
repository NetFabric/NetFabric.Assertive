using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AsyncEnumerableAssertionsBase<TActual> 
        : AssertionsBase<TActual> 
    {
        internal AsyncEnumerableAssertionsBase(TActual Actual, EnumerableInfo enumerableInfo) 
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
            var actualItemType = EnumerableInfo.ItemType;
            var wrapped = new AsyncEnumerableWrapper<TActual, TActualItem>(Actual, EnumerableInfo);
            switch (wrapped.Compare(expected, comparer, out var index))
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

        protected void AssertDeepEquality<TActualItem, TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsAsyncEnumerable(out var enumerableInfo))
                {
                    var wrapped = new AsyncEnumerableWrapper<TActual, TActualItem>(Actual, enumerableInfo);

#if !NETSTANDARD2_1 // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
                    if (enumerableInfo.ItemType.IsByRef)
                        continue;
#endif

                    switch (wrapped.Compare(expected, comparer, out var index))
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