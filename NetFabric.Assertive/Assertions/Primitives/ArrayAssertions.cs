using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class ArrayAssertions<TActualItem>
        : ReferenceTypeAssertions<TActualItem[]>
    {
        internal ArrayAssertions(TActualItem[] actual)
            : base(actual)
        {
        }

        public new ArrayAssertions<TActualItem> EvaluateTrue(Func<TActualItem[], bool> func)
            => this.EvaluateTrue<ArrayAssertions<TActualItem>, TActualItem[]>(func);

        public new ArrayAssertions<TActualItem> EvaluateFalse(Func<TActualItem[], bool> func)
            => this.EvaluateFalse<ArrayAssertions<TActualItem>, TActualItem[]>(func);

        [Obsolete("Use EvaluateTrue instead.")]
        public new ArrayAssertions<TActualItem> EvaluatesTrue(Func<TActualItem[], bool> func)
            => this.EvaluateTrue(func);

        [Obsolete("Use EvaluateFalse instead.")]
        public new ArrayAssertions<TActualItem> EvaluatesFalse(Func<TActualItem[], bool> func)
            => this.EvaluateFalse(func);

        public ArrayAssertions<TActualItem> BeArrayOf<TType>()
            => this.BeOfType<ArrayAssertions<TActualItem>, TActualItem[], TType[]>();

        public ArrayAssertions<TActualItem> NotBeArrayOf<TType>()
            => this.NotBeOfType<ArrayAssertions<TActualItem>, TActualItem[], TType[]>();

        public new ArrayAssertions<TActualItem> BeAssignableTo<TType>()
            => this.BeAssignableTo<ArrayAssertions<TActualItem>, TActualItem[], TType>();

        public new ArrayAssertions<TActualItem> BeNotAssignableTo<TType>()
            => this.BeNotAssignableTo<ArrayAssertions<TActualItem>, TActualItem[], TType>();

        public new ArrayAssertions<TActualItem> BeNull()
            => this.BeNull<ArrayAssertions<TActualItem>, TActualItem[]>();

        public new ArrayAssertions<TActualItem> BeNotNull()
            => this.BeNotNull<ArrayAssertions<TActualItem>, TActualItem[]>();

        public ArrayAssertions<TActualItem> BeSameAs<TExpected>(TExpected[] expected)
            => this.BeSameAs<ArrayAssertions<TActualItem>, TActualItem[], TExpected[]>(expected);

        public ArrayAssertions<TActualItem> BeNotSameAs<TExpected>(TExpected[] expected)
            => this.BeNotSameAs<ArrayAssertions<TActualItem>, TActualItem[], TExpected[]>(expected);

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
    }
}