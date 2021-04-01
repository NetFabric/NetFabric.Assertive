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
            => new(actual);

        [Pure]
        public static StringAssertions Must(this string? actual)
            => new(actual);

        [Pure]
        public static ArrayAssertions<TActual> Must<TActual>(this TActual[] actual)
            => new(actual);

        [Pure]
        public static StringArrayAssertions Must(this string[] actual)
            => new(actual);

        [Pure]
        public static ArraySegmentAssertions<TActual> Must<TActual>(this ArraySegment<TActual> actual)
            => new(actual);

        [Pure]
        public static ReadOnlyMemoryAssertions<TActual> Must<TActual>(this Span<TActual> actual)
            => Must((ReadOnlySpan<TActual>)actual);

        [Pure]
        public static ReadOnlyMemoryAssertions<TActual> Must<TActual>(this ReadOnlySpan<TActual> actual)
            => new(actual.ToArray().AsMemory());

        [Pure]
        public static ReadOnlyMemoryAssertions<TActual> Must<TActual>(this Memory<TActual> actual)
            => new(actual);

        [Pure]
        public static ReadOnlyMemoryAssertions<TActual> Must<TActual>(this ReadOnlyMemory<TActual> actual)
            => new(actual);

        [Pure]
        public static ActionAssertions Must(this Action actual)
            => new(actual);

        [Pure]
        public static FunctionAssertions<TActual> Must<TActual>(this Func<TActual> actual)
            => new(actual);

        [Pure]
        public static AsyncFunctionAssertions Must(this Func<ValueTask> actual)
            => new(actual);

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
            => new(actual);

        [Pure]
        public static NullableValueTypeAssertions<TActual> Must<TActual>(this TActual? actual)
            where TActual : struct
            => new(actual);
    }

    [DebuggerNonUserCode]
    public static class ReferenceTypeExtensions
    {
        [Pure]
        public static ReferenceTypeAssertions<TActual> Must<TActual>(this TActual actual) 
            where TActual : class
            => new(actual);
    }
}