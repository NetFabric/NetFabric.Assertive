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

        public NullableValueTypeAssertions<TActual> EvaluateTrue(Func<Nullable<TActual>, bool> func)
            => this.EvaluateTrue<NullableValueTypeAssertions<TActual>, Nullable<TActual>>(func);

        public NullableValueTypeAssertions<TActual> EvaluateFalse(Func<Nullable<TActual>, bool> func)
            => this.EvaluateFalse<NullableValueTypeAssertions<TActual>, Nullable<TActual>>(func);

        [Obsolete("Use EvaluateTrue instead.")]
        public NullableValueTypeAssertions<TActual> EvaluatesTrue(Func<Nullable<TActual>, bool> func)
            => this.EvaluateTrue(func);

        [Obsolete("Use EvaluateFalse instead.")]
        public NullableValueTypeAssertions<TActual> EvaluatesFalse(Func<Nullable<TActual>, bool> func)
            => this.EvaluateFalse(func);

        public NullableValueTypeAssertions<TActual> BeEqualTo(Nullable<TActual> expected)
            => this.BeEqualTo<NullableValueTypeAssertions<TActual>, Nullable<TActual>>(expected);

        public NullableValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<Nullable<TActual>, TExpected, bool> comparer)
            => this.BeEqualTo<NullableValueTypeAssertions<TActual>, Nullable<TActual>, TExpected>(expected, comparer);

        public NullableValueTypeAssertions<TActual> BeNotEqualTo(Nullable<TActual> expected)
            => this.BeNotEqualTo<NullableValueTypeAssertions<TActual>, Nullable<TActual>>(expected);

        public NullableValueTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<Nullable<TActual>, TExpected, bool> comparer)
            => this.BeNotEqualTo<NullableValueTypeAssertions<TActual>, Nullable<TActual>, TExpected>(expected, comparer);

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
    }
}
