using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableValueTypeAssertions<TActual, TActualItem> 
        : ValueTypeAssertionsBase<EnumerableValueTypeAssertions<TActual, TActualItem>, TActual>
        where TActual : struct
    {
        internal EnumerableValueTypeAssertions(TActual actual, EnumerableInfo enumerableInfo)
            : base(actual) 
            => EnumerableInfo = enumerableInfo;

        public EnumerableInfo EnumerableInfo { get; }

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEmpty(bool testRefReturns = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), testRefReturns);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, 
            bool testRefReturns = true, bool testNonGeneric = true, bool testIndexOf = true, IEnumerable<TActualItem>? doesNotContain = default)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), testRefReturns, testNonGeneric, testIndexOf, doesNotContain);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, 
            bool testRefReturns = true, bool testNonGeneric = true, bool testIndexOf = true, IEnumerable<TActualItem>? doesNotContain = default)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActual, TExpected?>(Actual, expected);

            AssertEnumerableEquality(Actual, EnumerableInfo, expected, comparer, testRefReturns, testNonGeneric, testIndexOf, doesNotContain);

            return this;
        }
    }
}