using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class StringArrayAssertions
        : ReferenceTypeAssertionsBase<string[]>
    {
        internal StringArrayAssertions(string[] actual)
            : base(actual)
        {
        }

        public StringArrayAssertions BeNull()
            => BeNull<StringArrayAssertions>(this);

        public StringArrayAssertions BeNotNull()
            => BeNotNull<StringArrayAssertions>(this);

        public StringArrayAssertions BeSameAs(string[] expected)
            => BeSameAs<StringArrayAssertions, string[]>(this, expected);

        public StringArrayAssertions BeNotSameAs(string[] expected)
            => BeNotSameAs<StringArrayAssertions, string[]>(this, expected);

        public StringArrayAssertions EvaluateTrue(Func<string[]?, bool> func)
            => EvaluateTrue<StringArrayAssertions>(this, func);

        public StringArrayAssertions EvaluateFalse(Func<string[]?, bool> func)
            => EvaluateFalse<StringArrayAssertions>(this, func);

        public StringArrayAssertions BeArrayOf<TType>()
            => BeOfType<StringArrayAssertions, TType[]>(this);

        public StringArrayAssertions NotBeArrayOf<TType>()
            => NotBeOfType<StringArrayAssertions, TType[]>(this);

        public StringArrayAssertions BeAssignableTo<TType>()
            => BeAssignableTo<StringArrayAssertions, TType>(this);

        public StringArrayAssertions BeNotAssignableTo<TType>()
            => BeNotAssignableTo<StringArrayAssertions, TType>(this);

        public StringArrayAssertions BeEqualTo<TExpected>(TExpected expected, bool ignoreCase = false)
            where TExpected : IEnumerable<string>
        {
            if (Actual is null)
            {
                if (expected is not null)
                    throw new EqualToAssertionException<string[], TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<string[], TExpected>(Actual, expected);

                var (result, index, actualItem, expectedItem) = Actual.Compare(expected);
                switch (result)
                {
                    case EqualityResult.NotEqualAtIndex:
                        {
                            if (actualItem is not null && expectedItem is not null)
                            {
                                var (_, stringIndex) = actualItem!.Compare(expectedItem!, ignoreCase);
                                throw new StringEqualToAssertionException(
                                    actualItem,
                                    expectedItem,
                                    stringIndex,
                                    $"Arrays differ at index {index}.");
                            }
                            else
                            {
                                throw new EqualToAssertionException<string[], TExpected>(
                                    Actual,
                                    expected,
                                    $"Arrays differ at index {index}.");
                            }
                        }

                    case EqualityResult.LessItem:
                        throw new EqualToAssertionException<string[], TExpected>(
                            Actual,
                            expected,
                            $"Actual array has less items.");

                    case EqualityResult.MoreItems:
                        throw new EqualToAssertionException<string[], TExpected>(
                            Actual,
                            expected,
                            $"Actual array has more items.");
                }
            }

            return this;
        }

        public StringArrayAssertions BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<string, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (Actual is null)
            {
                if (expected is not null)
                    throw new EqualToAssertionException<string[], TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<string[], TExpected>(Actual, expected);

                var (result, index, _, _) = Actual.Compare(expected, comparer);
                switch (result)
                {
                    case EqualityResult.NotEqualAtIndex:
                        throw new EqualToAssertionException<string[], TExpected>(
                            Actual,
                            expected,
                            $"Arrays differ at index {index}.");

                    case EqualityResult.LessItem:
                        throw new EqualToAssertionException<string[], TExpected>(
                            Actual,
                            expected,
                            $"Actual array has less items.");

                    case EqualityResult.MoreItems:
                        throw new EqualToAssertionException<string[], TExpected>(
                            Actual,
                            expected,
                            $"Actual array has more items.");
                }
            }

            return this;
        }
    }
}