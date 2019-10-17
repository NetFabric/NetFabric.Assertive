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

        protected void AssertEquality<TActualItem, TExpected>(TExpected expected, bool deepComparison)
            where TExpected : IEnumerable<TActualItem>
        {
#if !NETSTANDARD2_1 // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
            if (!EnumerableInfo.ItemType.IsByRef)
            {
#endif
                var getEnumeratorDeclaringType = EnumerableInfo.GetEnumerator.DeclaringType;
                var actualItemType = EnumerableInfo.ItemType;
                var wrapped = new AsyncEnumerableWrapper<TActual>(Actual, EnumerableInfo);
                switch (wrapped.Compare(expected, out var index))
                {
                    case EqualityResult.NotEqualAtIndex:
                        {
                            throw new AsyncEnumerableAssertionException<TActual, TExpected>(
                                wrapped,
                                expected,
                                $"Actual differs at index {index} when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                        }

                    case EqualityResult.LessItem:
                        {
                            throw new AsyncEnumerableAssertionException<TActual, TExpected>(
                                wrapped,
                                expected,
                                $"Actual has less items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                        }

                    case EqualityResult.MoreItems:
                        {
                            throw new AsyncEnumerableAssertionException<TActual, TExpected>(
                                wrapped,
                                expected,
                                $"Actual has more items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                        }
                }
#if !NETSTANDARD2_1
            }
#endif

            if (deepComparison)
            {
                foreach (var @interface in typeof(TActual).GetInterfaces())
                {
                    if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                    {
                        var wrappedInterface = new AsyncEnumerableWrapper<TActual>(Actual, interfaceEnumerableInfo);
                        switch (wrappedInterface.Compare(expected, out var interfaceIndex))
                        {
                            case EqualityResult.NotEqualAtIndex:
                                {
                                    throw new AsyncEnumerableAssertionException<TActual, TExpected>(
                                        wrappedInterface,
                                        expected,
                                        $"Actual differs at index {interfaceIndex} when using '{@interface}.GetEnumerator()'.");
                                }

                            case EqualityResult.LessItem:
                                {
                                    throw new AsyncEnumerableAssertionException<TActual, TExpected>(
                                        wrappedInterface,
                                        expected,
                                        $"Actual has less items when using '{@interface}.GetEnumerator()'.");
                                }

                            case EqualityResult.MoreItems:
                                {
                                    throw new AsyncEnumerableAssertionException<TActual, TExpected>(
                                        wrappedInterface,
                                        expected,
                                        $"Actual has more items when using '{@interface}.GetEnumerator()'.");
                                }
                        }
                    }
                }
            }
        }
    }
}