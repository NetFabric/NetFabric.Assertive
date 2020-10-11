using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> 
        : ReferenceTypeAssertionsBase<ExceptionAssertions<TException>, TException>
        where TException : Exception
    {
        internal ExceptionAssertions(TException actual) 
            : base(actual) 
        {
        }
    }
}