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

        public BooleanAssertions BeEqualTo(bool expected)
        {
            if (Actual != expected)
                throw new EqualToAssertionException<bool, bool>(Actual, expected);

            return this;
        }

        public BooleanAssertions BeEqualTo<TExpected>(TExpected expected, Func<bool, TExpected, bool> comparer)
        {
            if (!comparer(Actual, expected))
                throw new EqualToAssertionException<bool, TExpected>(Actual, expected);

            return this;
        }
    }
}