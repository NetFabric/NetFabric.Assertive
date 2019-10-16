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

        public new ExceptionAssertions<TException> EvaluatesTrue(Func<TException, bool> func)
        {
            if (!func(Actual))
                throw new ActualAssertionException<TException>(Actual,
                    $"Evaluates to 'false'.");

            return this;
        }

        public new ExceptionAssertions<TException> EvaluatesFalse(Func<TException, bool> func)
        {
            if (func(Actual))
                throw new ActualAssertionException<TException>(Actual,
                    $"Evaluates to 'true'.");

            return this;
        }
    }
}