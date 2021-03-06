﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReadOnlyMemoryAssertions<TActualItem>
        : ValueTypeAssertionsBase<ReadOnlyMemoryAssertions<TActualItem>, ReadOnlyMemory<TActualItem>>
    {
        internal ReadOnlyMemoryAssertions(ReadOnlyMemory<TActualItem> actual)
            : base(actual)
        {
        }

        public ReadOnlyMemoryAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public ReadOnlyMemoryAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActualItem[], TExpected>(Actual.ToArray(), expected);

            var (result, index, _, _) = Actual.Span.Compare(expected, comparer);
            return result switch
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