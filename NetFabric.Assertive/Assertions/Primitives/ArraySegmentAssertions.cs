using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ArraySegmentAssertions<TActualItem>
        : ValueTypeAssertionsBase<ArraySegmentAssertions<TActualItem>, ArraySegment<TActualItem>>
    {
        internal ArraySegmentAssertions(ArraySegment<TActualItem> actual)
            : base(actual)
        {
        }

        public ArraySegmentAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public ArraySegmentAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<ArraySegment<TActualItem>, TExpected>(Actual, expected);

            var (result, index, _, _) = Actual.AsSpan().Compare(expected, comparer);
            return result switch
            {
                EqualityResult.NotEqualAtIndex 
                    => throw new EqualToAssertionException<ArraySegment<TActualItem>, TExpected>(
                        Actual,
                        expected,
                        $"Collections differ at index {index}."),

                EqualityResult.LessItem 
                    => throw new EqualToAssertionException<ArraySegment<TActualItem>, TExpected>(
                        Actual,
                        expected,
                        $"Actual collection has less items."),

                EqualityResult.MoreItems 
                    => throw new EqualToAssertionException<ArraySegment<TActualItem>, TExpected>(
                        Actual,
                        expected,
                        $"Actual collection has more items."),

                _ => this,
            };
        }
    }
}