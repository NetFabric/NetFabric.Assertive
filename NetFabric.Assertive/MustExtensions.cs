using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class MustExtensions
    {
        [Pure]
        public static BooleanAssertions Must(this bool actual)
            => new BooleanAssertions(actual);

        [Pure]
        public static ArrayAssertions<TActual> Must<TActual>(this TActual[] actual)
            => new ArrayAssertions<TActual>(actual);

        [Pure]
        public static ActionAssertions Must(this Action actual)
            => new ActionAssertions(actual);

        [Pure]
        public static FunctionAssertions<TActual> Must<TActual>(this Func<TActual> actual)
            => new FunctionAssertions<TActual>(actual);
    }

    [DebuggerNonUserCode]
    public static class ValueTypeExtensions
    {
        [Pure]
        public static ValueTypeAssertions<TActual> Must<TActual>(this TActual actual) 
            where TActual : struct
            => new ValueTypeAssertions<TActual>(actual);

        [Pure]
        public static NullableValueTypeAssertions<TActual> Must<TActual>(this Nullable<TActual> actual)
            where TActual : struct
            => new NullableValueTypeAssertions<TActual>(actual);
    }

    [DebuggerNonUserCode]
    public static class ReferenceTypeExtensions
    {
        [Pure]
        public static ReferenceTypeAssertions<TActual> Must<TActual>(this TActual actual) 
            where TActual : class
            => new ReferenceTypeAssertions<TActual>(actual); 
    }
}