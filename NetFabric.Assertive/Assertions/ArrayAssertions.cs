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

        public ArrayAssertions<TActual> EvaluatesTrue(Func<TActual[], bool> func)
            => this.EvaluatesTrue<ArrayAssertions<TActual>, TActual[]>(func);

        public ArrayAssertions<TActual> EvaluatesFalse(Func<TActual[], bool> func)
            => this.EvaluatesFalse<ArrayAssertions<TActual>, TActual[]>(func);

        public ArrayAssertions<TActual> BeArrayOf<TType>()
            => this.BeOfType<ArrayAssertions<TActual>, TActual[], TType[]>();

        public ArrayAssertions<TActual> NotBeArrayOf<TType>()
            => this.NotBeOfType<ArrayAssertions<TActual>, TActual[], TType[]>();

        public ArrayAssertions<TActual> BeAssignableTo<TType>()
            => this.BeAssignableTo<ArrayAssertions<TActual>, TActual[], TType>();

        public ArrayAssertions<TActual> BeNotAssignableTo<TType>()
            => this.BeNotAssignableTo<ArrayAssertions<TActual>, TActual[], TType>();

        public ArrayAssertions<TActual> BeNull()
            => this.BeNull<ArrayAssertions<TActual>, TActual[]>();

        public ArrayAssertions<TActual> BeNotNull()
            => this.BeNotNull<ArrayAssertions<TActual>, TActual[]>();

        public ArrayAssertions<TActual> BeSameAs<TExpected>(TExpected[] expected)
            => this.BeSameAs<ArrayAssertions<TActual>, TActual[], TExpected[]>(expected);

        public ArrayAssertions<TActual> BeNotSameAs<TExpected>(TExpected[] expected)
            => this.BeNotSameAs<ArrayAssertions<TActual>, TActual[], TExpected[]>(expected);

        public new ArrayAssertions<TActual> BeEqualTo(TActual[] expected)
            => BeEqualTo<TActual[], TActual>(expected, (actual, expected) => EqualityComparer<TActual>.Default.Equals(actual, expected));

        public ArrayAssertions<TActual> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActual, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<TActual[], TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual[], TExpected>(Actual, expected);

                switch (Actual.Compare(expected, comparer, out var index))
                {
                    case EqualityResult.NotEqualAtIndex:
                        throw new EqualToAssertionException<TActual[], TExpected>(
                            Actual,
                            expected,
                            $"Arrays differ at index {index}.");

                    case EqualityResult.LessItem:
                        throw new EqualToAssertionException<TActual[], TExpected>(
                            Actual,
                            expected,
                            $"Actual array has less items.");

                    case EqualityResult.MoreItems:
                        throw new EqualToAssertionException<TActual[], TExpected>(
                            Actual,
                            expected,
                            $"Actual array has more items.");
                }
            }

            return this;
        }
    }
}