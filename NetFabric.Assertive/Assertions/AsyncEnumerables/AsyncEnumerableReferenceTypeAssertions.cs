using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> 
        : AsyncEnumerableAssertionsBase<TActual>
        where TActual : class
    {
        internal AsyncEnumerableReferenceTypeAssertions(TActual actual, EnumerableInfo enumerableInfo) 
            : base(actual, enumerableInfo)
        {
        }

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeEmpty(bool deepComparison = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), deepComparison);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, bool deepComparison = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), deepComparison);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, bool deepComparison = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

                AssertEquality(expected, comparer);

                if (deepComparison)
                    AssertDeepEquality(expected, comparer);
            }

            return this;
        }
    }
}