using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class NullableValueTypeAssertions<TActual>
        : AssertionsBase
        where TActual : struct
    {
        internal NullableValueTypeAssertions(TActual? actual)
        {
            Actual = actual;
        }

        public TActual? Actual { get; }

        public NullableValueTypeAssertions<TActual> EvaluateTrue(Func<TActual?, bool> func)
        {
            if (!func(Actual))
                throw new ActualAssertionException<TActual?>(Actual, $"Evaluates to 'false'.");

            return this;
        }

        public NullableValueTypeAssertions<TActual> EvaluateFalse(Func<TActual?, bool> func)
        {
            if (func(Actual))
                throw new ActualAssertionException<TActual?>(Actual, $"Evaluates to 'true'.");

            return this;
        }

        public NullableValueTypeAssertions<TActual> BeEqualTo(TActual? expected)
        {
            if (!EqualityComparer<TActual?>.Default.Equals(Actual, expected))
                throw new EqualToAssertionException<TActual?, TActual?>(Actual, expected);

            return this;
        }

        public NullableValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual?, TExpected, bool> comparer)
        {
            if (!comparer(Actual, expected))
                throw new EqualToAssertionException<TActual?, TExpected>(Actual, expected);

            return this;
        }

        public NullableValueTypeAssertions<TActual> BeNotEqualTo(TActual? expected)
        {
            if (EqualityComparer<TActual?>.Default.Equals(Actual, expected))
                throw new NotEqualToAssertionException<TActual?, TActual?>(Actual, expected);

            return this;
        }

        public NullableValueTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<TActual?, TExpected, bool> comparer)
        {
            if (comparer(Actual, expected))
                throw new NotEqualToAssertionException<TActual?, TExpected>(Actual, expected);

            return this;
        }

        public NullableValueTypeAssertions<TActual> HaveValue()
        {
            if (!Actual.HasValue)
                throw new NotEqualToAssertionException<TActual?, TActual?>(null, Actual);

            return this;
        }

        public NullableValueTypeAssertions<TActual> NotHaveValue()
        {
            if (Actual.HasValue)
                throw new EqualToAssertionException<TActual?, TActual?>(Actual, null);

            return this;
        }
    }
}
