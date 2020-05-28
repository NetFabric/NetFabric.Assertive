using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncEnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestAsyncEnumerable, int[], string> BeEqualTo_NotEqualNullData =>
            new TheoryData<TestAsyncEnumerable, int[], string>
            {
                { null, TestData.Empty, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: <null>" },
                { new TestAsyncEnumerable(TestData.Empty), null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Null_Should_Throw(TestAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must()
                .BeAsyncEnumerableOf<int>()
                .BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<TestAsyncEnumerable, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<TestAsyncEnumerable, int[], string> BeEqualTo_AsyncEnumerable_NotEqualData =>
            new TheoryData<TestAsyncEnumerable, int[], string>
            {
                { new TestAsyncEnumerable(TestData.Empty), TestData.Single, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestAsyncEnumerable(TestData.Single), TestData.Empty, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestAsyncEnumerable(TestData.Single), TestData.SingleNotEqual, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestAsyncEnumerable(TestData.Single), TestData.Multiple, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestAsyncEnumerable(TestData.Multiple), TestData.Empty, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestAsyncEnumerable(TestData.Multiple), TestData.Single, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestAsyncEnumerable(TestData.Multiple), TestData.MultipleNotEqualFirst, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestAsyncEnumerable(TestData.Multiple), TestData.MultipleNotEqualMiddle, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestAsyncEnumerable(TestData.Multiple), TestData.MultipleNotEqualLast, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_AsyncEnumerable_NotEqualData))]
        public void BeEqualTo_AsyncEnumerable_With_NotEqual_Should_Throw(TestAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<AsyncEnumerableAssertionException<TestAsyncEnumerable, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<TestCancellableAsyncEnumerable, int[], string> BeEqualTo_AsyncCancellableEnumerable_NotEqualData =>
            new TheoryData<TestCancellableAsyncEnumerable, int[], string>
            {
                { new TestCancellableAsyncEnumerable(TestData.Empty), TestData.Single, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestCancellableAsyncEnumerable(TestData.Single), TestData.Empty, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestCancellableAsyncEnumerable(TestData.Single), TestData.SingleNotEqual, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestCancellableAsyncEnumerable(TestData.Single), TestData.Multiple, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestCancellableAsyncEnumerable(TestData.Multiple), TestData.Empty, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestCancellableAsyncEnumerable(TestData.Multiple), TestData.Single, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestCancellableAsyncEnumerable(TestData.Multiple), TestData.MultipleNotEqualFirst, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestCancellableAsyncEnumerable(TestData.Multiple), TestData.MultipleNotEqualMiddle, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestCancellableAsyncEnumerable(TestData.Multiple), TestData.MultipleNotEqualLast, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },

                { new TestGenericAsyncEnumerable(TestData.Empty), TestData.Single, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestGenericAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestGenericAsyncEnumerable(TestData.Single), TestData.Empty, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestGenericAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestGenericAsyncEnumerable(TestData.Single), TestData.SingleNotEqual, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestGenericAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestGenericAsyncEnumerable(TestData.Single), TestData.Multiple, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestGenericAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple), TestData.Empty, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestGenericAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple), TestData.Single, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestGenericAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple), TestData.MultipleNotEqualFirst, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestGenericAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple), TestData.MultipleNotEqualMiddle, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestGenericAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple), TestData.MultipleNotEqualLast, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestGenericAsyncEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_AsyncEnumerable_NotEqualData))]
        public void BeEqualTo_AsyncCancellableEnumerable_With_NotEqual_Should_Throw(TestCancellableAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<AsyncEnumerableAssertionException<TestAsyncEnumerable, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

    }
}