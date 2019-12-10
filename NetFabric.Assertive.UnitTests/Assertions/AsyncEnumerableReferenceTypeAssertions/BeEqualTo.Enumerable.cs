using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncEnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeAsyncEnumerable, int[]> Enumerable_EqualData =>
            new TheoryData<RangeAsyncEnumerable, int[]> 
            {
                { null, null },
                { new RangeAsyncEnumerable(0), new int[] { } },
                { new RangeAsyncEnumerable(1), new int[] { 0 } },
                { new RangeAsyncEnumerable(3), new int[] { 0, 1, 2 } },
                { new RangeGenericAsyncEnumerable(3, 0), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(Enumerable_EqualData))]
        public void Enumerable_BeEqualTo_With_Equal_Should_NotThrow(RangeAsyncEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeAsyncEnumerable, int[], string> Enumerable_NotEqualData =>
            new TheoryData<RangeAsyncEnumerable, int[], string> 
            {
                { new RangeAsyncEnumerable(0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.RangeAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeAsyncEnumerable(1), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.RangeAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new RangeAsyncEnumerable(3), new int[] { 0, 5, 2 }, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.RangeAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualData))]
        public void Enumerable_BeEqualTo_With_NotEqual_Should_Throw(RangeAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<AsyncEnumerableAssertionException<RangeAsyncEnumerable, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<RangeAsyncEnumerable, int[], string> Enumerable_NotEqualNullData =>
            new TheoryData<RangeAsyncEnumerable, int[], string>
            {
                { null, new int[] { }, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: <null>" },
                { new RangeAsyncEnumerable(0), null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualNullData))]
        public void Enumerable_BeEqualTo_With_NotEqual_Null_Should_Throw(RangeAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeAsyncEnumerable, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}