using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AsyncEnumerableValueTypeAssertions<TActual, TActualItem> 
        : AsyncEnumerableAssertionsBase<TActual>
        where TActual : struct
    {
        internal AsyncEnumerableValueTypeAssertions(TActual actual, EnumerableInfo enumerableInfo) 
            : base(actual, enumerableInfo)
        {
        }

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeEmpty(bool deepComparison = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), deepComparison);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, bool deepComparison = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), deepComparison);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, bool deepComparison = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected), $"{typeof(TActual)} is a value type so it can't be expected to be <null>.");


            AssertEquality(expected, comparer);

            if (deepComparison)
                AssertDeepEquality(expected, comparer);

            return this;
        }

    }
}