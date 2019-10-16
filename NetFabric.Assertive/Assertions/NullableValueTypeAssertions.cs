using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class NullableValueTypeAssertions<TActual>
        : AssertionsBase<Nullable<TActual>>
        where TActual : struct
    {
        internal NullableValueTypeAssertions(Nullable<TActual> actual)
            : base(actual)
        {
        }

        public NullableValueTypeAssertions<TActual> EvaluatesTrue(Func<Nullable<TActual>, bool> func)
        {
            if (!func(Actual))
                throw new ActualAssertionException<Nullable<TActual>>(Actual,
                    $"Evaluates to 'false'.");

            return this;
        }

        public NullableValueTypeAssertions<TActual> EvaluatesFalse(Func<Nullable<TActual>, bool> func)
        {
            if (func(Actual))
                throw new ActualAssertionException<Nullable<TActual>>(Actual,
                    $"Evaluates to 'true'.");

            return this;
        }

        public NullableValueTypeAssertions<TActual> HaveValue()
        {
            if (!Actual.HasValue)
                throw new NotEqualToAssertionException<Nullable<TActual>, Nullable<TActual>>(Actual, null);

            return this;
        }

        public NullableValueTypeAssertions<TActual> NotHaveValue()
        {
            if (Actual.HasValue)
                throw new EqualToAssertionException<Nullable<TActual>, Nullable<TActual>>(Actual, null);

            return this;
        }

        public NullableValueTypeAssertions<TActual> BeEqualTo(Nullable<TActual> expected)
        {
            if (!EqualityComparer<Nullable<TActual>>.Default.Equals(Actual, expected))
                throw new EqualToAssertionException<Nullable<TActual>, Nullable<TActual>>(Actual, expected);

            return this;
        }

        public NullableValueTypeAssertions<TActual> BeNotEqualTo(Nullable<TActual> expected)
        {
            if (EqualityComparer<Nullable<TActual>>.Default.Equals(Actual, expected))
                throw new NotEqualToAssertionException<Nullable<TActual>, Nullable<TActual>>(Actual, expected);

            return this;
        }
    }
}
