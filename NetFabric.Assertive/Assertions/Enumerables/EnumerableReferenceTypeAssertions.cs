using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableReferenceTypeAssertions<TActual, TActualItem> 
        : ReferenceTypeAssertionsBase<TActual>
        where TActual : class
    {
        internal EnumerableReferenceTypeAssertions(TActual? Actual, EnumerableInfo enumerableInfo)
            : base(Actual) 
            => EnumerableInfo = enumerableInfo;

        public EnumerableInfo EnumerableInfo { get; }

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeNull()
            => BeNull<EnumerableReferenceTypeAssertions<TActual, TActualItem>>(this);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeNotNull()
            => BeNotNull<EnumerableReferenceTypeAssertions<TActual, TActualItem>>(this);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeSameAs<TOther>(TOther? other)
            => BeSameAs<EnumerableReferenceTypeAssertions<TActual, TActualItem>, TOther>(this, other);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeNotSameAs<TOther>(TOther? other)
            => BeNotSameAs<EnumerableReferenceTypeAssertions<TActual, TActualItem>, TOther>(this, other);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluateTrue(Func<TActual?, bool> func)
            => EvaluateTrue<EnumerableReferenceTypeAssertions<TActual, TActualItem>>(this, func);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluateFalse(Func<TActual?, bool> func)
            => EvaluateFalse<EnumerableReferenceTypeAssertions<TActual, TActualItem>>(this, func);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEmpty(bool testRefStructs = true, bool testRefReturns = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), testRefStructs, testRefReturns);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected? expected, bool testRefStructs = true, bool testRefReturns = true, bool testNonGeneric = true, bool testIndexOf = true, IEnumerable<TActualItem>? doesNotContain = default)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual!, expected!), testRefStructs, testRefReturns, testNonGeneric, testIndexOf, doesNotContain);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected? expected, Func<TActualItem?, TExpectedItem?, bool> comparer, 
            bool testRefStructs = true, bool testRefReturns = true, bool testNonGeneric = true, bool testIndexOf = true, IEnumerable<TActualItem>? doesNotContain = default)
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

                AssertEnumerableEquality(Actual, EnumerableInfo, expected, comparer, testRefStructs, testRefReturns, testNonGeneric, testIndexOf, doesNotContain);
            }

            return this;
        }
    }
}