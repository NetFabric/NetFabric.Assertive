using System;
using System.Linq;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestCollection, int[]> TestCollection_EqualData =>
            new TheoryData<TestCollection, int[]>
            {
                { new TestCollection(TestData.Empty),    TestData.Empty },
                { new TestCollection(TestData.Single),   TestData.Single },
                { new TestCollection(TestData.Multiple), TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(TestCollection_EqualData))]
        public void BeEqualTo_TestCollection_With_Equal_Should_NotThrow(TestCollection actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestCollection, int[], string> BeEqualTo_TestCollection_NotEqualCountData =>
            new TheoryData<TestCollection, int[], string>
            {
                { new TestCollection(TestData.Empty,    TestData.SingleCount,   TestData.Single),   TestData.Empty,     $"Expected collections to have same count value.{Environment.NewLine}Expected: {TestData.EmptyCount}{Environment.NewLine}Actual: {TestData.SingleCount}" },
                { new TestCollection(TestData.Single,   TestData.EmptyCount,    TestData.Empty),    TestData.Single,    $"Expected collections to have same count value.{Environment.NewLine}Expected: {TestData.SingleCount}{Environment.NewLine}Actual: {TestData.EmptyCount}" },
                { new TestCollection(TestData.Single,   TestData.MultipleCount, TestData.Multiple), TestData.Single,    $"Expected collections to have same count value.{Environment.NewLine}Expected: {TestData.SingleCount}{Environment.NewLine}Actual: {TestData.MultipleCount}" },
                { new TestCollection(TestData.Multiple, TestData.EmptyCount,    TestData.Empty),    TestData.Multiple,  $"Expected collections to have same count value.{Environment.NewLine}Expected: {TestData.MultipleCount}{Environment.NewLine}Actual: {TestData.EmptyCount}" },
                { new TestCollection(TestData.Multiple, TestData.SingleCount,   TestData.Single),   TestData.Multiple,  $"Expected collections to have same count value.{Environment.NewLine}Expected: {TestData.MultipleCount}{Environment.NewLine}Actual: {TestData.SingleCount}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestCollection_NotEqualCountData))]
        public void BeEqualTo_TestCollection_With_NotEqualCount_Should_Throw(TestCollection actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<CountAssertionException>(action);
            Assert.Equal(actual.Count, exception.Actual);
            Assert.Equal(expected.Length, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<TestCollection, int[], int[], string> BeEqualTo_TestCollection_NotEqualCopyToData =>
            new TheoryData<TestCollection, int[], int[], string>
            {
                { new TestCollection(TestData.Single,   TestData.SingleCount,   TestData.SingleNotEqual),         TestData.SingleNotEqual,         TestData.Single,    $"Actual differs at index 0 when using the CopyTo.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.SingleNotEqual.ToFriendlyString()}" },
                { new TestCollection(TestData.Multiple, TestData.MultipleCount, TestData.MultipleNotEqualFirst),  TestData.MultipleNotEqualFirst,  TestData.Multiple,  $"Actual differs at index 0 when using the CopyTo.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { new TestCollection(TestData.Multiple, TestData.MultipleCount, TestData.MultipleNotEqualMiddle), TestData.MultipleNotEqualMiddle, TestData.Multiple,  $"Actual differs at index 2 when using the CopyTo.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { new TestCollection(TestData.Multiple, TestData.MultipleCount, TestData.MultipleNotEqualLast),   TestData.MultipleNotEqualLast,   TestData.Multiple,  $"Actual differs at index 4 when using the CopyTo.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestCollection_NotEqualCopyToData))]
        public void BeEqualTo_TestCollection_With_NotEqualCopyTo_Should_Throw(TestCollection source, int[] actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => source.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<CopyToWrapper<int>, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}