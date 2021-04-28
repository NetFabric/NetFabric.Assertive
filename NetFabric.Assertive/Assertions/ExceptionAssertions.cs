using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ExceptionAssertions<TException> 
        : NullableReferenceTypeAssertionsBase<ExceptionAssertions<TException>, TException>
        where TException : Exception
    {
        internal ExceptionAssertions(TException actual) 
            : base(actual) 
        {
        }
    }
}