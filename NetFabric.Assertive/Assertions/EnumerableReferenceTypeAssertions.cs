using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableReferenceTypeAssertions<TActual, TActualItem> 
        where TActual : class
    {
        internal EnumerableReferenceTypeAssertions(TActual actual, EnumerableInfo enumerableInfo) 
        {
            Actual = actual;
            EnumerableInfo = enumerableInfo;
        }

        public TActual Actual { get; }
        public EnumerableInfo EnumerableInfo { get; }

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEmpty()
            => BeEqualTo(Enumerable.Empty<TActualItem>());

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo(IEnumerable<TActualItem> expected)
            => BeEqualTo<TActualItem>(expected);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpectedItem>(IEnumerable<TExpectedItem> expected)
            => BeEqualTo(expected, (actual, expected) => actual.Equals(expected));

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpectedItem>(IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<TActual, IEnumerable<TExpectedItem>>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new NullException<TActual>();
            }

            EqualityComparer.AssertEquality<TActual, TActualItem, TExpectedItem>(Actual, EnumerableInfo, expected, equalityComparison);

            return this;
        }
    }
}