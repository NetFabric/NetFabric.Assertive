using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> 
        : ReferenceTypeAssertions<TException>
        where TException
        : Exception
    {
        internal ExceptionAssertions(TException actual) 
            : base(actual) 
        {
        }

        public ExceptionAssertions<TException> WithMessage(string expected) 
        {
            if (Actual.Message != expected)
                throw new ExpectedAssertionException<string, string>(Actual.Message, expected, 
                    $"Expected message '{expected}' but found '{Actual.Message}' instead.");

            return this;
        }

        public ExceptionAssertions<TException> EvaluatesTrue(Func<TException, bool> func)
        {
            if (!func(Actual))
                throw new ActualAssertionException<TException>(Actual,
                    $"Evaluates to 'false'.");

            return this;
        }
    }
}