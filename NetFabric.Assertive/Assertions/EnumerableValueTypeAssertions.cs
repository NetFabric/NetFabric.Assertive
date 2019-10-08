using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableValueTypeAssertions<TActual, TActualItem> 
        where TActual : struct
    {
        internal EnumerableValueTypeAssertions(TActual actual, EnumerableInfo enumerableInfo) 
        {
            Actual = actual;
            EnumerableInfo = enumerableInfo;
        }

        public TActual Actual { get; }
        public EnumerableInfo EnumerableInfo { get; }

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEmpty()
            => BeEqualTo(Enumerable.Empty<TActualItem>());

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo(IEnumerable<TActualItem> expected)
            => BeEqualTo<TActualItem>(expected);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpectedItem>(IEnumerable<TExpectedItem> expected)
            => BeEqualTo(expected, (actual, expected) => actual.Equals(expected));

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpectedItem>(IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected), $"{typeof(TActual)} is a value type so it can't be expected to be <null>.");

            EqualityComparer.AssertEquality(Actual, EnumerableInfo, expected, equalityComparison);

            return this;
        }

    }
}