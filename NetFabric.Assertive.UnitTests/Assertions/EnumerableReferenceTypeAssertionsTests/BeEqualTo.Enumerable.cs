using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeEnumerable, int[]> Enumerable_EqualData =>
            new TheoryData<RangeEnumerable, int[]> 
            {
                { null, null },
                { new RangeEnumerable(0), new int[] { } },
                { new RangeEnumerable(1), new int[] { 0 } },
                { new RangeEnumerable(3), new int[] { 0, 1, 2 } },
                { new RangeNonGenericEnumerable(3, 0), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(Enumerable_EqualData))]
        public void Enumerable_BeEqualTo_With_Equal_Should_NotThrow(RangeEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeEnumerable, int[], string> Enumerable_NotEqualData =>
            new TheoryData<RangeEnumerable, int[], string> 
            {
                { new RangeEnumerable(0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeEnumerable(1), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new RangeEnumerable(3), new int[] { 0, 5, 2 }, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualData))]
        public void Enumerable_BeEqualTo_With_NotEqual_Should_Throw(RangeEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<RangeEnumerable, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<RangeEnumerable, int[], string> Enumerable_NotEqualNullData =>
            new TheoryData<RangeEnumerable, int[], string>
            {
                { null, new int[] { }, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: <null>" },
                { new RangeEnumerable(0), null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualNullData))]
        public void Enumerable_BeEqualTo_With_NotEqual_Null_Should_Throw(RangeEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeEnumerable, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}