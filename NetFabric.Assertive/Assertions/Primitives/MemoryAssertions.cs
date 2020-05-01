﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class MemoryAssertions<TActualItem>
        : ValueTypeAssertionsBase<Memory<TActualItem>>
    {
        internal MemoryAssertions(Memory<TActualItem> actual)
            : base(actual)
        {
        }

        public MemoryAssertions<TActualItem> EvaluateTrue(Func<Memory<TActualItem>, bool> func)
            => EvaluateTrue<MemoryAssertions<TActualItem>>(this, func);

        public MemoryAssertions<TActualItem> EvaluateFalse(Func<Memory<TActualItem>, bool> func)
            => EvaluateFalse<MemoryAssertions<TActualItem>>(this, func);

        public MemoryAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public MemoryAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActualItem[], TExpected>(Actual.ToArray(), expected);

            return (((ReadOnlyMemory<TActualItem>)Actual).Span.Compare(expected, comparer, out var index)) switch
            {
                EqualityResult.NotEqualAtIndex 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Memory differ at index {index}."),

                EqualityResult.LessItem 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual Memory has less items."),

                EqualityResult.MoreItems 
                    => throw new EqualToAssertionException<TActualItem[], TExpected>(
                        Actual.ToArray(),
                        expected,
                        $"Actual Memory has more items."),

                _ => this,
            };
        }
    }
}