using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public ref struct ReadOnlySpanAssertions<TActualItem>
    {
        internal ReadOnlySpanAssertions(ReadOnlySpan<TActualItem> actual)
        {
            Actual = actual;
        }

        public ReadOnlySpan<TActualItem> Actual { get; }

        public ReadOnlySpanAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public ReadOnlySpanAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActualItem[], TExpected>(Actual.ToArray(), expected);

            return (Actual.Compare(expected, comparer, out var index)) switch
            {
                EqualityResult.NotEqualAtIndex 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"ReadOnlySpan differ at index {index}."),

                EqualityResult.LessItem 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual ReadOnlySpan has less items."),

                EqualityResult.MoreItems 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual ReadOnlySpan has more items."),

                _ => this,
            };
        }
    }
}