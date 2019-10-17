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
            => this.EvaluatesTrue<ExceptionAssertions<TException>, TException>(func);

        public new ExceptionAssertions<TException> EvaluatesFalse(Func<TException, bool> func)
            => this.EvaluatesFalse<ExceptionAssertions<TException>, TException>(func);
    }
}