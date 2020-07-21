﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class ArraySegmentAssertions<TActualItem>
        : ValueTypeAssertionsBase<ArraySegment<TActualItem>>
    {
        internal ArraySegmentAssertions(ArraySegment<TActualItem> actual)
            : base(actual)
        {
        }

        public ArraySegmentAssertions<TActualItem> EvaluateTrue(Func<ArraySegment<TActualItem>, bool> func)
            => EvaluateTrue<ArraySegmentAssertions<TActualItem>>(this, func);

        public ArraySegmentAssertions<TActualItem> EvaluateFalse(Func<ArraySegment<TActualItem>, bool> func)
            => EvaluateFalse<ArraySegmentAssertions<TActualItem>>(this, func);

        public ArraySegmentAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public ArraySegmentAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<ArraySegment<TActualItem>, TExpected>(Actual, expected);

            return (Actual.AsSpan().Compare(expected, comparer, out var index)) switch
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