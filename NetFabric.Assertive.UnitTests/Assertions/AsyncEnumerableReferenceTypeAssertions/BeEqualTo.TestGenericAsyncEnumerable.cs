using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncEnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestGenericAsyncEnumerable, int[]> BeEqualTo_TestGenericAsyncEnumerable_EqualData 
            => new()
            {
                { new TestGenericAsyncEnumerable(TestData.Empty),       TestData.Empty },
                { new TestGenericAsyncEnumerable(TestData.Single),      TestData.Single },
                { new TestGenericAsyncEnumerable(TestData.Multiple),    TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestGenericAsyncEnumerable_EqualData))]
        public void BeEqualTo_TestGenericAsyncEnumerable_With_Equal_Should_NotThrow(TestCancellableAsyncEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestGenericAsyncEnumerable, int[], string> BeEqualTo_TestGenericAsyncEnumerable_NotEqualData 
            => new()
            {
                { new TestGenericAsyncEnumerable(TestData.Empty,    TestData.Single),                   TestData.Empty,     $"Actual has more items when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestGenericAsyncEnumerable(TestData.Single,   TestData.Empty),                    TestData.Single,    $"Actual has less items when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestGenericAsyncEnumerable(TestData.Single,   TestData.SingleNotEqual),           TestData.Single,    $"Actual differs at index 0 when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.SingleNotEqual.ToFriendlyString()}" },
                { new TestGenericAsyncEnumerable(TestData.Single,   TestData.Multiple),                 TestData.Single,    $"Actual differs at index 0 when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple, TestData.Empty),                    TestData.Multiple,  $"Actual has less items when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple, TestData.Single),                   TestData.Multiple,  $"Actual differs at index 0 when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple, TestData.MultipleNotEqualFirst),    TestData.Multiple,  $"Actual differs at index 0 when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple, TestData.MultipleNotEqualMiddle),   TestData.Multiple,  $"Actual differs at index 2 when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { new TestGenericAsyncEnumerable(TestData.Multiple, TestData.MultipleNotEqualLast),     TestData.Multiple,  $"Actual differs at index 4 when using 'System.Collections.Generic.IAsyncEnumerable`1[System.Int32].GetAsyncEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestGenericAsyncEnumerable_NotEqualData))]
        public void BeEqualTo_TestGenericAsyncEnumerable_With_NotEqual_Should_Throw(TestGenericAsyncEnumerable actual, int[] expected, string message)
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
