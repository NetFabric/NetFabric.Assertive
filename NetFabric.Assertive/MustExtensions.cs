using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

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

#if NETCORE

        [Pure]
        public static SpanAssertions<TActual> Must<TActual>(this Span<TActual> actual)
            => new SpanAssertions<TActual>(actual.ToArray());

        [Pure]
        public static ReadOnlySpanAssertions<TActual> Must<TActual>(this ReadOnlySpan<TActual> actual)
            => new ReadOnlySpanAssertions<TActual>(actual.ToArray());

        [Pure]
        public static MemoryAssertions<TActual> Must<TActual>(this Memory<TActual> actual)
            => new MemoryAssertions<TActual>(actual);

        [Pure]
        public static ReadOnlyMemoryAssertions<TActual> Must<TActual>(this ReadOnlyMemory<TActual> actual)
            => new ReadOnlyMemoryAssertions<TActual>(actual);

#endif

        [Pure]
        public static ActionAssertions Must(this Action actual)
            => new ActionAssertions(actual);

        [Pure]
        public static FunctionAssertions<TActual> Must<TActual>(this Func<TActual> actual)
            => new FunctionAssertions<TActual>(actual);

        [Pure]
        public static AsyncFunctionAssertions Must(this Func<ValueTask> actual)
            => new AsyncFunctionAssertions(actual);

        [Pure]
        public static AsyncFunctionAssertions Must(this Func<Task> actual)
            => Must(() => new ValueTask(actual()));
    }

    [DebuggerNonUserCode]
    public static class ValueTypeExtensions
    {
        [Pure]
        public static ValueTypeAssertions<TActual> Must<TActual>(this TActual actual) 
            where TActual : struct
            => new ValueTypeAssertions<TActual>(actual);

        [Pure]
        public static NullableValueTypeAssertions<TActual> Must<TActual>(this TActual? actual)
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