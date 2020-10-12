using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class BooleanAssertions
        : ValueTypeAssertionsBase<BooleanAssertions, bool>
    {
        internal BooleanAssertions(bool actual)
            : base(actual)
        {
        }

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