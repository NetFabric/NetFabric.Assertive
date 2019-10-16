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
        {
            if (!func(Actual))
                throw new ActualAssertionException<TActual>(Actual,
                    $"Evaluates to 'false'.");

            return this;
        }

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluatesFalse(Func<TActual, bool> func)
        {
            if (func(Actual))
                throw new ActualAssertionException<TActual>(Actual,
                    $"Evaluates to 'true'.");

            return this;
        }

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
                    throw new EqualToAssertionException<TActual, IEnumerable<TExpectedItem>>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual, IEnumerable<TExpectedItem>>(Actual, expected);

                EqualityComparer.AssertEquality<TActual, TActualItem, TExpected, TExpectedItem>(Actual, EnumerableInfo, expected);
            }

            return this;
        }

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeSameAs(TActual expected)
        {
            if (!Object.ReferenceEquals(Actual, expected))
                throw new ExpectedAssertionException<TActual, TActual>(Actual, expected,
                    $"Expected '{Actual.ToFriendlyString()}' to be same as '{expected.ToFriendlyString()}' but it's not.");

            return this;
        }

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeNotSameAs(TActual expected)
        {
            if (Object.ReferenceEquals(Actual, expected))
                throw new ExpectedAssertionException<TActual, TActual>(Actual, expected,
                    $"Expected '{Actual.ToFriendlyString()}' to be not same as '{expected.ToFriendlyString()}' but it is.");

            return this;
        }
    }
}