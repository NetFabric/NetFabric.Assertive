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

        public new ExceptionAssertions<TException> EvaluateTrue(Func<TException, bool> func)
            => this.EvaluateTrue<ExceptionAssertions<TException>, TException>(func);

        public new ExceptionAssertions<TException> EvaluateFalse(Func<TException, bool> func)
            => this.EvaluateFalse<ExceptionAssertions<TException>, TException>(func);

        [Obsolete("Use EvaluateTrue instead.")]
        public new ExceptionAssertions<TException> EvaluatesTrue(Func<TException, bool> func)
            => this.EvaluateTrue(func);

        [Obsolete("Use EvaluatesFalse instead.")]
        public new ExceptionAssertions<TException> EvaluatesFalse(Func<TException, bool> func)
            => this.EvaluateFalse(func);
    }
}