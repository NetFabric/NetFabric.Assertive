using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncEnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestGenericAsyncEnumerable, int[]> GenericEnumerable_EqualData =>
            new TheoryData<TestGenericAsyncEnumerable, int[]> 
            {
                { null, null },
                { new TestGenericAsyncEnumerable(0, 0), new int[] { } },
                { new TestGenericAsyncEnumerable(1, 1), new int[] { 0 } },
                { new TestGenericAsyncEnumerable(3, 3), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(GenericEnumerable_EqualData))]
        public void GenericEnumerable_BeEqualTo_With_Equal_Should_NotThrow(TestGenericAsyncEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestGenericAsyncEnumerable, int[], string> GenericEnumerable_NotEqualData =>
            new TheoryData<TestGenericAsyncEnumerable, int[], string> 
            {
                { new TestGenericAsyncEnumerable(0, 0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.RangeAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestGenericAsyncEnumerable(1, 0), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.RangeAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new TestGenericAsyncEnumerable(1, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestGenericAsyncEnumerable(0, 1), new int[] { }, $"Actual has more items when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
            };

        [Theory]
        [MemberData(nameof(GenericEnumerable_NotEqualData))]
        public void GenericEnumerable_BeEqualTo_With_NotEqual_Should_Throw(TestGenericAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<AsyncEnumerableAssertionException<TestGenericAsyncEnumerable, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}