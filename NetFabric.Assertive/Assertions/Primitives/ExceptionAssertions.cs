using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> 
        : ReferenceTypeAssertionsBase<TException>
        where TException
        : Exception
    {
        internal ExceptionAssertions(TException actual) 
            : base(actual) 
        {
        }

        public ExceptionAssertions<TException> EvaluateTrue(Func<TException, bool> func)
            => EvaluateTrue<ExceptionAssertions<TException>>(this, func);

        public ExceptionAssertions<TException> EvaluateFalse(Func<TException, bool> func)
            => EvaluateFalse<ExceptionAssertions<TException>>(this, func);
    }
}