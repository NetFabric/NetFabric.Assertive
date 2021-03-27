using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class StringArrayAssertions
        : ArrayAssertions<string>
    {
        internal StringArrayAssertions(string[] actual)
            : base(actual)
        {
        }

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

                var (result, index, actualItem, expectedItem) = Actual.Compare<string[], string, TExpected>(expected);
                switch (result)
                {
                    case EqualityResult.NotEqualAtIndex:
                        {
                            if (actualItem is not null && expectedItem is not null)
                            {
                                var (_, stringIndex) = actualItem!.Compare(expectedItem!, ignoreCase);
                                var (line, character) = actualItem.IndexToLineCharacter(stringIndex);
                                throw new StringEqualToAssertionException(
                                    actualItem,
                                    expectedItem,
                                    stringIndex,
                                    $"Arrays differ at index {index}, in line {line} character {character}.");
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
    }
}