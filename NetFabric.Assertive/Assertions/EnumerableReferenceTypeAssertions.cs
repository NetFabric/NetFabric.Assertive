using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableReferenceTypeAssertions<TActual, TActualItem> 
        : EnumerableAssertionsBase<TActual>
        where TActual : class
    {
        internal EnumerableReferenceTypeAssertions(TActual actual, EnumerableInfo enumerableInfo) 
            : base(actual, enumerableInfo)
        {
        }

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
                    throw new EqualToAssertionException<TActual, IEnumerable<TExpectedItem>>(Actual, expected);

                EqualityComparer.AssertEquality(Actual, EnumerableInfo, expected, equalityComparison);
            }

            return this;
        }
    }
}