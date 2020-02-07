using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class MemoryAssertions<TActualItem>
        : ValueTypeAssertions<ReadOnlyMemory<TActualItem>>
    {
        internal MemoryAssertions(ReadOnlyMemory<TActualItem> actual)
            : base(actual)
        {
        }

        public new MemoryAssertions<TActualItem> EvaluateTrue(Func<ReadOnlyMemory<TActualItem>, bool> func)
            => this.EvaluateTrue<MemoryAssertions<TActualItem>, ReadOnlyMemory<TActualItem>>(func);

        public new MemoryAssertions<TActualItem> EvaluateFalse(Func<ReadOnlyMemory<TActualItem>, bool> func)
            => this.EvaluateFalse<MemoryAssertions<TActualItem>, ReadOnlyMemory<TActualItem>>(func);

        public new MemoryAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public MemoryAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActualItem[], TExpected>(Actual.ToArray(), expected);

            switch (Actual.Span.Compare(expected, comparer, out var index))
            {
                case EqualityResult.NotEqualAtIndex:
                    throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Memories differ at index {index}.");

                case EqualityResult.LessItem:
                    throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual memory has less items.");

                case EqualityResult.MoreItems:
                    throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual memory has more items.");
            }

            return this;
        }
    }
}