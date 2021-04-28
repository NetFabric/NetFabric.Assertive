using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AsyncEnumerableNullableReferenceTypeAssertions<TActual, TActualItem>
        : NullableReferenceTypeAssertionsBase<AsyncEnumerableNullableReferenceTypeAssertions<TActual, TActualItem>, TActual>
        where TActual : class?
    {
        internal AsyncEnumerableNullableReferenceTypeAssertions(TActual actual, AsyncEnumerableInfo enumerableInfo)
            : base(actual) 
            => EnumerableInfo = enumerableInfo;

        public AsyncEnumerableInfo EnumerableInfo { get; }

        public AsyncEnumerableNullableReferenceTypeAssertions<TActual, TActualItem> BeEmpty(bool testRefReturns = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), testRefReturns);

        public AsyncEnumerableNullableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, bool testRefReturns = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), testRefReturns);

        public AsyncEnumerableNullableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, bool testRefReturns = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (Actual is null)
            {
                if (expected is not null)
                    throw new EqualToAssertionException<TActual?, TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual, TExpected?>(Actual, expected);

                AssertAsyncEnumerableEquality(Actual, EnumerableInfo, expected, comparer, testRefReturns);
            }

            return this;
        }
    }
}