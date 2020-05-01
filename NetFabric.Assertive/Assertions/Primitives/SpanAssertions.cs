using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public ref struct SpanAssertions<TActualItem>
    {
        internal SpanAssertions(Span<TActualItem> actual)
        {
            Actual = actual;
        }

        public Span<TActualItem> Actual { get; }

        public SpanAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public SpanAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActualItem[], TExpected>(Actual.ToArray(), expected);

            return (((ReadOnlySpan<TActualItem>)Actual).Compare(expected, comparer, out var index)) switch
            {
                EqualityResult.NotEqualAtIndex 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Span differ at index {index}."),

                EqualityResult.LessItem 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual Span has less items."),

                EqualityResult.MoreItems 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual Span has more items."),

                _ => this,
            };
        }
    }
}