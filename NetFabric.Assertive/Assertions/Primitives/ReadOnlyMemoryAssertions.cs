using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class ReadOnlyMemoryAssertions<TActualItem>
        : ValueTypeAssertionsBase<ReadOnlyMemory<TActualItem>>
    {
        internal ReadOnlyMemoryAssertions(ReadOnlyMemory<TActualItem> actual)
            : base(actual)
        {
        }

        public ReadOnlyMemoryAssertions<TActualItem> EvaluateTrue(Func<ReadOnlyMemory<TActualItem>, bool> func)
            => EvaluateTrue<ReadOnlyMemoryAssertions<TActualItem>>(this, func);

        public ReadOnlyMemoryAssertions<TActualItem> EvaluateFalse(Func<ReadOnlyMemory<TActualItem>, bool> func)
            => EvaluateFalse<ReadOnlyMemoryAssertions<TActualItem>>(this, func);

        public ReadOnlyMemoryAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public ReadOnlyMemoryAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActualItem[], TExpected>(Actual.ToArray(), expected);

            return (Actual.Span.Compare(expected, comparer, out var index)) switch
            {
                EqualityResult.NotEqualAtIndex 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Collections differ at index {index}."),

                EqualityResult.LessItem 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual collection has less items."),

                EqualityResult.MoreItems 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual collection has more items."),

                _ => this,
            };
        }
    }
}