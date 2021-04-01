using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableReferenceTypeAssertions<TActual, TActualItem> 
        : ReferenceTypeAssertionsBase<EnumerableReferenceTypeAssertions<TActual, TActualItem>, TActual>
        where TActual : class
    {
        internal EnumerableReferenceTypeAssertions(TActual actual, EnumerableInfo enumerableInfo)
            : base(actual) 
            => EnumerableInfo = enumerableInfo;

        public EnumerableInfo EnumerableInfo { get; }

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEmpty(bool testRefStructs = true, bool testRefReturns = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), testRefStructs, testRefReturns);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected,bool testNonGeneric = true, bool testIndexOf = true, IEnumerable<TActualItem>? doesNotContain = default)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual!, expected!), testNonGeneric, testIndexOf, doesNotContain);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem?, TExpectedItem?, bool> comparer, 
            bool testNonGeneric = true, bool testIndexOf = true, IEnumerable<TActualItem>? doesNotContain = default)
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

                AssertEnumerableEquality(Actual, EnumerableInfo, expected, comparer, testNonGeneric, testIndexOf, doesNotContain);
            }

            return this;
        }
    }
}