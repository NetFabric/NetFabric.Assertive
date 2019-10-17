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

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluatesTrue(Func<TActual, bool> func)
            => this.EvaluatesTrue<EnumerableReferenceTypeAssertions<TActual, TActualItem>, TActual>(func);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluatesFalse(Func<TActual, bool> func)
            => this.EvaluatesFalse<EnumerableReferenceTypeAssertions<TActual, TActualItem>, TActual>(func);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> NotShareState()
        {
            if (Actual is object)
                EqualityComparer.AssertNotSharing<TActual, TActualItem>(Actual, EnumerableInfo);

            return this;
        }

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEmpty()
            => BeEqualTo(Enumerable.Empty<TActualItem>());

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

                EqualityComparer.AssertEquality<TActual, TActualItem, TExpected, TExpectedItem>(Actual, EnumerableInfo, expected);
            }

            return this;
        }
    }
}