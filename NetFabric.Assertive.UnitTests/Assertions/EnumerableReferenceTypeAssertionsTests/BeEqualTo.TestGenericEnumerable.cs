using System;
using NetFabric.Reflection;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestGenericEnumerable, int[]> BeEqualTo_TestGenericEnumerable_EqualData =>
            new()
            {
                { new TestGenericEnumerable(TestData.Empty),    TestData.Empty },
                { new TestGenericEnumerable(TestData.Single),   TestData.Single },
                { new TestGenericEnumerable(TestData.Multiple), TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestGenericEnumerable_EqualData))]
        public void BeEqualTo_TestGenericEnumerable_With_Equal_Should_NotThrow(TestGenericEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestGenericEnumerable, int[], string> BeEqualTo_TestGenericEnumerable_NotEqualData =>
            new()
            {
                { new TestGenericEnumerable(TestData.Empty,    TestData.Single),                 TestData.Empty,     $"Actual has more items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestGenericEnumerable(TestData.Single,   TestData.Empty),                  TestData.Single,    $"Actual has less items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestGenericEnumerable(TestData.Single,   TestData.SingleNotEqual),         TestData.Single,    $"Actual differs at index 0 when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.SingleNotEqual.ToFriendlyString()}" },
                { new TestGenericEnumerable(TestData.Single,   TestData.Multiple),               TestData.Single,    $"Actual differs at index 0 when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { new TestGenericEnumerable(TestData.Multiple, TestData.Empty),                  TestData.Multiple,  $"Actual has less items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestGenericEnumerable(TestData.Multiple, TestData.Single),                 TestData.Multiple,  $"Actual differs at index 0 when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestGenericEnumerable(TestData.Multiple, TestData.MultipleNotEqualFirst),  TestData.Multiple,  $"Actual differs at index 0 when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { new TestGenericEnumerable(TestData.Multiple, TestData.MultipleNotEqualMiddle), TestData.Multiple,  $"Actual differs at index 2 when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { new TestGenericEnumerable(TestData.Multiple, TestData.MultipleNotEqualLast),   TestData.Multiple,  $"Actual differs at index 4 when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestGenericEnumerable_NotEqualData))]
        public void BeEqualTo_TestGenericEnumerable_With_NotEqual_Should_Throw(TestGenericEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<EnumerableWrapper<TestGenericEnumerable, int>, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}