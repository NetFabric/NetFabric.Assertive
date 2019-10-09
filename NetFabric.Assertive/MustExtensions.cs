using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class ValueTypeExtensions
    {
        [Pure]
        public static ValueTypeAssertions<TActual> Must<TActual>(this TActual actual) 
            where TActual : struct
            => new ValueTypeAssertions<TActual>(actual); 
    }

    [DebuggerNonUserCode]
    public static class ReferenceTypeExtensions
    {
        [Pure]
        public static ReferenceTypeAssertions<TActual> Must<TActual>(this TActual actual) 
            where TActual : class
            => new ReferenceTypeAssertions<TActual>(actual); 
    }

    [DebuggerNonUserCode]
    public static class ActionExtensions
    {

        [Pure]
        public static ActionAssertions Must(this Action actual) 
            => new ActionAssertions(actual); 
    }
}