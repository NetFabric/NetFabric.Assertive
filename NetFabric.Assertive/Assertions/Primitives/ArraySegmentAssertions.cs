using System;
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

        public ArraySegmentAssertions<TActualItem> BeArraySegmentOf<TType>()
            => BeOfType<ArraySegmentAssertions<TActualItem>, TType[]>(this);

        public ArraySegmentAssertions<TActualItem> NotBeArraySegmentOf<TType>()
            => NotBeOfType<ArraySegmentAssertions<TActualItem>, TType[]>(this);

        public ArraySegmentAssertions<TActualItem> BeAssignableTo<TType>()
            => BeAssignableTo<ArraySegmentAssertions<TActualItem>, TType>(this);

        public ArraySegmentAssertions<TActualItem> BeNotAssignableTo<TType>()
            => BeNotAssignableTo<ArraySegmentAssertions<TActualItem>, TType>(this);

        public ArraySegmentAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public ArraySegmentAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<ArraySegment<TActualItem>, TExpected>(Actual, expected);

            switch (Actual.Compare(expected, comparer, out var index))
            {
                case EqualityResult.NotEqualAtIndex:
                    throw new EqualToAssertionException<ArraySegment<TActualItem>, TExpected>(
                        Actual,
                        expected,
                        $"Array segments differ at index {index}.");

                case EqualityResult.LessItem:
                    throw new EqualToAssertionException<ArraySegment<TActualItem>, TExpected>(
                        Actual,
                        expected,
                        $"Actual array segment has less items.");

                case EqualityResult.MoreItems:
                    throw new EqualToAssertionException<ArraySegment<TActualItem>, TExpected>(
                        Actual,
                        expected,
                        $"Actual array segment has more items.");
            }

            return this;
        }

        public EnumerableValueTypeAssertions<ArraySegment<TActualItem>, TExpectedItem> BeEnumerableOf<TExpectedItem>()
        {
            AssertIsEnumerable<ArraySegment<TActualItem>, TExpectedItem>(Actual, out var enumerableInfo);

            return new EnumerableValueTypeAssertions<ArraySegment<TActualItem>, TExpectedItem>(Actual, enumerableInfo);
        }
    }
}