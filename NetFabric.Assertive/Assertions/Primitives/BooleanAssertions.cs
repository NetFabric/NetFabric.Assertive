using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class BooleanAssertions
        : ValueTypeAssertionsBase<bool>
    {
        internal BooleanAssertions(bool actual)
            : base(actual)
        {
        }

        public BooleanAssertions BeEqualTo(bool expected)
            => BeEqualTo(this, expected);

        public BooleanAssertions BeEqualTo<TExpected>(TExpected expected, Func<bool, TExpected, bool> comparer)
            => BeEqualTo(this, expected, comparer);

        public BooleanAssertions BeNotEqualTo(bool expected)
            => BeNotEqualTo(this, expected);

        public BooleanAssertions BeNotEqualTo<TExpected>(TExpected expected, Func<bool, TExpected, bool> comparer)
            => BeNotEqualTo(this, expected, comparer);

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