using System;
using NetFabric.Reflection;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestNonGenericEnumerable, int[]> BeEqualTo_TestNonGenericEnumerable_EqualData =>
            new()
            {
                { new TestNonGenericEnumerable(TestData.Empty),     TestData.Empty },
                { new TestNonGenericEnumerable(TestData.Single),    TestData.Single },
                { new TestNonGenericEnumerable(TestData.Multiple),  TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestNonGenericEnumerable_EqualData))]
        public void BeEqualTo_TestNonGenericEnumerable_With_Equal_Should_NotThrow(TestNonGenericEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestNonGenericEnumerable, int[], string> BeEqualTo_TestNonGenericEnumerable_NotEqualData =>
            new()
            {
                { new TestNonGenericEnumerable(TestData.Empty,    TestData.Single),                 TestData.Empty,     $"Actual has more items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestNonGenericEnumerable(TestData.Single,   TestData.Empty),                  TestData.Single,    $"Actual has less items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestNonGenericEnumerable(TestData.Single,   TestData.SingleNotEqual),         TestData.Single,    $"Actual differs at index 0 when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.SingleNotEqual.ToFriendlyString()}" },
                { new TestNonGenericEnumerable(TestData.Single,   TestData.Multiple),               TestData.Single,    $"Actual differs at index 0 when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { new TestNonGenericEnumerable(TestData.Multiple, TestData.Empty),                  TestData.Multiple,  $"Actual has less items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestNonGenericEnumerable(TestData.Multiple, TestData.Single),                 TestData.Multiple,  $"Actual differs at index 0 when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestNonGenericEnumerable(TestData.Multiple, TestData.MultipleNotEqualFirst),  TestData.Multiple,  $"Actual differs at index 0 when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { new TestNonGenericEnumerable(TestData.Multiple, TestData.MultipleNotEqualMiddle), TestData.Multiple,  $"Actual differs at index 2 when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { new TestNonGenericEnumerable(TestData.Multiple, TestData.MultipleNotEqualLast),   TestData.Multiple,  $"Actual differs at index 4 when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestNonGenericEnumerable_NotEqualData))]
        public void BeEqualTo_TestNonGenericEnumerable_With_NotEqual_Should_Throw(TestNonGenericEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<EnumerableWrapper<TestNonGenericEnumerable, int>, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}