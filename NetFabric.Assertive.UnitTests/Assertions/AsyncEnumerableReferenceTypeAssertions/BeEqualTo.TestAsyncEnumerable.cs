using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncEnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestAsyncEnumerable, int[], string> BeEqualTo_NotEqualNullData =>
            new TheoryData<TestAsyncEnumerable, int[], string>
            {
                { null, TestData.Empty, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: <null>" },
                { new TestAsyncEnumerable(TestData.Empty), null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
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
                { new TestAsyncEnumerable(TestData.Single),                 TestData.Empty,     $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestAsyncEnumerable(TestData.Empty),                  TestData.Single,    $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestAsyncEnumerable(TestData.SingleNotEqual),         TestData.Single,    $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.SingleNotEqual.ToFriendlyString()}" },
                { new TestAsyncEnumerable(TestData.Multiple),               TestData.Single,    $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { new TestAsyncEnumerable(TestData.Empty),                  TestData.Multiple,  $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestAsyncEnumerable(TestData.Single),                 TestData.Multiple,  $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestAsyncEnumerable(TestData.MultipleNotEqualFirst),  TestData.Multiple,  $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { new TestAsyncEnumerable(TestData.MultipleNotEqualMiddle), TestData.Multiple,  $"Actual differs at index 2 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { new TestAsyncEnumerable(TestData.MultipleNotEqualLast),   TestData.Multiple,  $"Actual differs at index 4 when using 'NetFabric.Assertive.UnitTests.TestAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
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
    }
}