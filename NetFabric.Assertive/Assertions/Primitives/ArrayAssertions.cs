using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class ArrayAssertions<TActualItem>
        : ReferenceTypeAssertionsBase<TActualItem[]>
    {
        internal ArrayAssertions(TActualItem[] actual)
            : base(actual)
        {
        }

        public ArrayAssertions<TActualItem> BeNull()
            => BeNull<ArrayAssertions<TActualItem>>(this);

        public ArrayAssertions<TActualItem> BeNotNull()
            => BeNotNull<ArrayAssertions<TActualItem>>(this);

        public ArrayAssertions<TActualItem> BeSameAs<TExpected>(TExpected[] expected)
            => BeSameAs<ArrayAssertions<TActualItem>, TExpected[]>(this, expected);

        public ArrayAssertions<TActualItem> BeNotSameAs<TExpected>(TExpected[] expected)
            => BeNotSameAs<ArrayAssertions<TActualItem>, TExpected[]>(this, expected);

        public ArrayAssertions<TActualItem> EvaluateTrue(Func<TActualItem[], bool> func)
            => EvaluateTrue<ArrayAssertions<TActualItem>>(this, func);

        public ArrayAssertions<TActualItem> EvaluateFalse(Func<TActualItem[], bool> func)
            => EvaluateFalse<ArrayAssertions<TActualItem>>(this, func);

        public ArrayAssertions<TActualItem> BeArrayOf<TType>()
            => BeOfType<ArrayAssertions<TActualItem>, TType[]>(this);

        public ArrayAssertions<TActualItem> NotBeArrayOf<TType>()
            => NotBeOfType<ArrayAssertions<TActualItem>, TType[]>(this);

        public ArrayAssertions<TActualItem> BeAssignableTo<TType>()
            => BeAssignableTo<ArrayAssertions<TActualItem>, TType>(this);

        public ArrayAssertions<TActualItem> BeNotAssignableTo<TType>()
            => BeNotAssignableTo<ArrayAssertions<TActualItem>, TType>(this);

        public ArrayAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public ArrayAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<TActualItem[], TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActualItem[], TExpected>(Actual, expected);

                switch (Actual.Compare(expected, comparer, out var index))
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