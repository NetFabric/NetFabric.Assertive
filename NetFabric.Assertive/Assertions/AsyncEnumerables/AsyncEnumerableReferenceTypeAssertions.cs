using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>
        : ReferenceTypeAssertionsBase<AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>, TActual>
        where TActual : class
    {
        internal AsyncEnumerableReferenceTypeAssertions(TActual? Actual, AsyncEnumerableInfo enumerableInfo)
            : base(Actual) 
            => EnumerableInfo = enumerableInfo;

        public AsyncEnumerableInfo EnumerableInfo { get; }

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeEmpty(bool testRefStructs = true, bool testRefReturns = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), testRefStructs, testRefReturns);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected? expected, bool testRefStructs = true, bool testRefReturns = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), testRefStructs, testRefReturns);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected? expected, Func<TActualItem, TExpectedItem, bool> comparer, bool testRefStructs = true, bool testRefReturns = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (Actual is null)
            {
                if (expected is not null)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

                AssertAsyncEnumerableEquality(Actual, EnumerableInfo, expected, comparer, testRefStructs, testRefReturns);
            }

            return this;
        }
    }
}