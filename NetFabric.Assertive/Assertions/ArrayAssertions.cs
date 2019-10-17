using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ArrayAssertions<TActual>
        : ReferenceTypeAssertions<TActual[]>
    {
        internal ArrayAssertions(TActual[] actual)
            : base(actual)
        {
        }

        public new ArrayAssertions<TActual> BeNull()
            => this.BeNull<ArrayAssertions<TActual>, TActual[]>();

        public new ArrayAssertions<TActual> BeNotNull()
            => this.BeNotNull<ArrayAssertions<TActual>, TActual[]>();

        public ArrayAssertions<TActual> BeSameAs<TExpected>(TExpected[] expected)
            => this.BeSameAs<ArrayAssertions<TActual>, TActual[], TExpected[]>(expected);

        public ArrayAssertions<TActual> BeNotSameAs<TExpected>(TExpected[] expected)
            => this.BeNotSameAs<ArrayAssertions<TActual>, TActual[], TExpected[]>(expected);

        public new ArrayAssertions<TActual> BeEqualTo(TActual[] expected)
            => BeEqualTo<TActual>(expected, (actual, expected) => EqualityComparer<TActual>.Default.Equals(actual, expected));

        public ArrayAssertions<TActual> BeEqualTo<TExpected>(TExpected[] expected, Func<TActual, TExpected, bool> comparer)
        {
            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<TActual[], TExpected[]>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual[], TExpected[]>(Actual, expected);

                (var result, var index) = Actual.Compare(expected, comparer);
                switch (result)
                {
                    case EqualityResult.NotEqualAtIndex:
                        throw new EqualToAssertionException<TActual[], TExpected[]>(
                            Actual,
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{Actual.ToFriendlyString()}' that differs at index {index}.");

                    case EqualityResult.LessItem:
                        throw new EqualToAssertionException<TActual[], TExpected[]>(
                            Actual,
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{Actual.ToFriendlyString()}' with less items.");

                    case EqualityResult.MoreItems:
                        throw new EqualToAssertionException<TActual[], TExpected[]>(
                            Actual,
                            expected,
                            $"Expected '{expected.ToFriendlyString()}' but found '{Actual.ToFriendlyString()}' with more items.");
                }
            }

            return this;
        }
    }
}