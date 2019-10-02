using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> 
        : ObjectAssertions<TException>
        where TException
        : Exception
    {
        readonly Exception actual;

        internal ExceptionAssertions(TException actual) 
            : base(actual) 
        {
            this.actual = actual;
        }

        public ExceptionAssertions<TException> WithMessage(string expected) 
        {
            if (actual.Message != expected)
                throw new ExpectedAssertionException<string, string>(actual.Message, expected, 
                    $"Expected message '{expected}' but found '{actual.Message}' instead.");

            return this;
        }
    }
}