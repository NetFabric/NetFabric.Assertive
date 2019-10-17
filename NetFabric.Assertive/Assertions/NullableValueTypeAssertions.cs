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

        public new NullableValueTypeAssertions<TActual> EvaluatesTrue(Func<Nullable<TActual>, bool> func)
            => this.EvaluatesTrue<NullableValueTypeAssertions<TActual>, Nullable<TActual>>(func);

        public new NullableValueTypeAssertions<TActual> EvaluatesFalse(Func<Nullable<TActual>, bool> func)
            => this.EvaluatesFalse<NullableValueTypeAssertions<TActual>, Nullable<TActual>>(func);

        public new NullableValueTypeAssertions<TActual> BeEqualTo(Nullable<TActual> expected)
            => this.BeEqualTo<NullableValueTypeAssertions<TActual>, Nullable<TActual>>(expected);

        public new NullableValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<Nullable<TActual>, TExpected, bool> comparer)
            => this.BeEqualTo<NullableValueTypeAssertions<TActual>, Nullable<TActual>, TExpected>(expected, comparer);

        public new NullableValueTypeAssertions<TActual> BeNotEqualTo(Nullable<TActual> expected)
            => this.BeNotEqualTo<NullableValueTypeAssertions<TActual>, Nullable<TActual>>(expected);

        public new NullableValueTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<Nullable<TActual>, TExpected, bool> comparer)
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
