using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncEnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestCancellableAsyncEnumerable, int[]> BeEqualTo_AsyncCancellableEnumerable_EqualData =>
       new()
       {
            { new TestCancellableAsyncEnumerable(TestData.Empty),       TestData.Empty },
            { new TestCancellableAsyncEnumerable(TestData.Single),      TestData.Single },
            { new TestCancellableAsyncEnumerable(TestData.Multiple),    TestData.Multiple },
       };

        [Theory]
        [MemberData(nameof(BeEqualTo_AsyncCancellableEnumerable_EqualData))]
        public void BeEqualTo_AsyncCancellableEnumerable_With_Equal_Should_NotThrow(TestCancellableAsyncEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestCancellableAsyncEnumerable, int[], string> BeEqualTo_AsyncCancellableEnumerable_NotEqualData =>
            new()
            {
                { new TestCancellableAsyncEnumerable(TestData.Single),                 TestData.Empty,     $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestCancellableAsyncEnumerable(TestData.Empty),                  TestData.Single,    $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestCancellableAsyncEnumerable(TestData.SingleNotEqual),         TestData.Single,    $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.SingleNotEqual.ToFriendlyString()}" },
                { new TestCancellableAsyncEnumerable(TestData.Multiple),               TestData.Single,    $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { new TestCancellableAsyncEnumerable(TestData.Empty),                  TestData.Multiple,  $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestCancellableAsyncEnumerable(TestData.Single),                 TestData.Multiple,  $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestCancellableAsyncEnumerable(TestData.MultipleNotEqualFirst),  TestData.Multiple,  $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { new TestCancellableAsyncEnumerable(TestData.MultipleNotEqualMiddle), TestData.Multiple,  $"Actual differs at index 2 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { new TestCancellableAsyncEnumerable(TestData.MultipleNotEqualLast),   TestData.Multiple,  $"Actual differs at index 4 when using 'NetFabric.Assertive.UnitTests.TestCancellableAsyncEnumerable.GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_AsyncCancellableEnumerable_NotEqualData))]
        public void BeEqualTo_AsyncCancellableEnumerable_With_NotEqual_Should_Throw(TestCancellableAsyncEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<AsyncEnumerableAssertionException<TestCancellableAsyncEnumerable, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}