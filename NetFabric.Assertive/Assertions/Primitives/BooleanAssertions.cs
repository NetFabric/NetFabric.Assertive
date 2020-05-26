using System;
using System.Diagnostics;

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
            => Actual 
                ? this 
                : throw new EqualToAssertionException<bool, bool>(Actual, true);

        public BooleanAssertions BeFalse() 
            => Actual 
                ? throw new EqualToAssertionException<bool, bool>(Actual, false) 
                : this;
    }
}