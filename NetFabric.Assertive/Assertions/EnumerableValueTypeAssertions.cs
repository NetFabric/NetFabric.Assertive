using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableValueTypeAssertions<TActual, TActualItem> 
        : EnumerableAssertionsBase<TActual>
        where TActual : struct
    {
        internal EnumerableValueTypeAssertions(TActual actual, EnumerableInfo enumerableInfo) 
            : base(actual, enumerableInfo)
        {
        }

        public EnumerableValueTypeAssertions<TActual, TActualItem> EvaluatesTrue(Func<TActual, bool> func)
        {
            if (!func(Actual))
                throw new ActualAssertionException<TActual>(Actual,
                    $"Evaluates to 'false'.");

            return this;
        }

        public EnumerableValueTypeAssertions<TActual, TActualItem> EvaluatesFalse(Func<TActual, bool> func)
        {
            if (func(Actual))
                throw new ActualAssertionException<TActual>(Actual,
                    $"Evaluates to 'true'.");

            return this;
        }

        public EnumerableValueTypeAssertions<TActual, TActualItem> NotShareState()
        {
            EqualityComparer.AssertNotSharing<TActual, TActualItem>(Actual, EnumerableInfo);

            return this;
        }

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEmpty()
            => BeEqualTo(Enumerable.Empty<TActualItem>());

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected), $"{typeof(TActual)} is a value type so it can't be expected to be <null>.");

            EqualityComparer.AssertEquality<TActual, TActualItem, TExpected, TExpectedItem>(Actual, EnumerableInfo, expected);

            return this;
        }

    }
}