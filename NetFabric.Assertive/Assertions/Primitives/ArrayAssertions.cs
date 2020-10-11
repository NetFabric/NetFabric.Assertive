using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ArrayAssertions<TActualItem>
        : ReferenceTypeAssertionsBase<ArrayAssertions<TActualItem>, TActualItem[]>
    {
        internal ArrayAssertions(TActualItem[] actual)
            : base(actual)
        {
        }

        public ArrayAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public ArrayAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (Actual is null)
            {
                if (expected is not null)
                    throw new EqualToAssertionException<TActualItem[], TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActualItem[], TExpected>(Actual, expected);

                var (result, index, _, _) = Actual.Compare(expected, comparer);
                switch (result)
                {
                    case EqualityResult.NotEqualAtIndex:
                        throw new EqualToAssertionException<TActualItem[], TExpected>(
                            Actual,
                            expected,
                            $"Arrays differ at index {index}.");

                    case EqualityResult.LessItem:
                        throw new EqualToAssertionException<TActualItem[], TExpected>(
                            Actual,
                            expected,
                            $"Actual array has less items.");

                    case EqualityResult.MoreItems:
                        throw new EqualToAssertionException<TActualItem[], TExpected>(
                            Actual,
                            expected,
                            $"Actual array has more items.");
                }
            }

            return this;
        }

        public EnumerableReferenceTypeAssertions<TActualItem[], TExpectedItem> BeEnumerableOf<TExpectedItem>()
        {
            AssertIsEnumerable<TActualItem[], TExpectedItem>(Actual, out var enumerableInfo);

            return new EnumerableReferenceTypeAssertions<TActualItem[], TExpectedItem>(Actual, enumerableInfo);
        }
    }
}