using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class BooleanAssertions
        : AssertionsBase<bool>
    {
        internal BooleanAssertions(bool actual)
            : base(actual)
        {
        }

        public BooleanAssertions BeEqualTo(bool expected)
            => this.BeEqualTo<BooleanAssertions, bool>(expected);

        public BooleanAssertions BeEqualTo<TExpected>(TExpected expected, Func<bool, TExpected, bool> comparer)
            => this.BeEqualTo<BooleanAssertions, bool, TExpected>(expected, comparer);

        public BooleanAssertions BeNotEqualTo(bool expected)
            => this.BeNotEqualTo<BooleanAssertions, bool>(expected);

        public BooleanAssertions BeNotEqualTo<TExpected>(TExpected expected, Func<bool, TExpected, bool> comparer)
            => this.BeNotEqualTo<BooleanAssertions, bool, TExpected>(expected, comparer);

        public BooleanAssertions BeTrue()
        {
            if (!Actual)
                throw new EqualToAssertionException<bool, bool>(Actual, true);

            return this;
        }

        public BooleanAssertions BeFalse()
        {
            if (Actual)
                throw new EqualToAssertionException<bool, bool>(Actual, false);

            return this;
        }
    }
}