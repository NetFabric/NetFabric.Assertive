using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncEnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<CancellableRangeAsyncEnumerable, int[]> CancellableEnumerable_EqualData =>
            new TheoryData<CancellableRangeAsyncEnumerable, int[]> 
            {
                { null, null },
                { new CancellableRangeAsyncEnumerable(0), new int[] { } },
                { new CancellableRangeAsyncEnumerable(1), new int[] { 0 } },
                { new CancellableRangeAsyncEnumerable(3), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(CancellableEnumerable_EqualData))]
        public void CancellableEnumerable_BeEqualTo_With_Equal_Should_NotThrow(CancellableRangeAsyncEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<CancellableRangeAsyncEnumerable, int[], string> CancellableEnumerable_NotEqualData =>
            new TheoryData<CancellableRangeAsyncEnumerable, int[], string> 
            {
                { new CancellableRangeAsyncEnumerable(0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.CancellableRangeAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new CancellableRangeAsyncEnumerable(1), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.CancellableRangeAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new CancellableRangeAsyncEnumerable(3), new int[] { 0, 5, 2 }, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.CancellableRangeAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
            };

        [Theory]
        [MemberData(nameof(CancellableEnumerable_NotEqualData))]
        public void CancellableEnumerable_BeEqualTo_With_NotEqual_Should_Throw(CancellableRangeAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<AsyncEnumerableAssertionException<CancellableRangeAsyncEnumerable, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<CancellableRangeAsyncEnumerable, int[], string> CancellableEnumerable_NotEqualNullData =>
            new TheoryData<CancellableRangeAsyncEnumerable, int[], string>
            {
                { null, new int[] { }, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: <null>" },
                { new CancellableRangeAsyncEnumerable(0), null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
            };

        [Theory]
        [MemberData(nameof(CancellableEnumerable_NotEqualNullData))]
        public void CancellableEnumerable_BeEqualTo_With_NotEqual_Null_Should_Throw(CancellableRangeAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<CancellableRangeAsyncEnumerable, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}