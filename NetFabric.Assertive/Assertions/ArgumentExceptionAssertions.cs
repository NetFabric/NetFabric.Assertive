using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ArgumentExceptionAssertions<TException> 
        : ExceptionAssertions<TException>
        where TException
        : ArgumentException
    {
        internal ArgumentExceptionAssertions(TException actual) 
            : base(actual) 
        {
        }

        public ArgumentExceptionAssertions<TException> WithParamName(string expected) 
        {
            if (Actual.Message != expected)
                throw new ExpectedAssertionException<string, string>(Actual.ParamName, expected, 
                    $"Expected parameter name '{expected}' but found '{Actual.ParamName}' instead.");

            return this;
        }
    }
}