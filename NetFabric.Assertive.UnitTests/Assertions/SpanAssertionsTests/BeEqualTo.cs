using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class SpanAssertionsTests
    {
        public static TheoryData<int[]> EqualData =>
            new()
            {
                { TestData.Empty },
                { TestData.Single },
                { TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(EqualData))]
        public void BeEqualTo_With_Equal_Should_NotThrow(int[] value)
        {
            // Arrange

            // Act
            _ = value.AsSpan().Must().BeEqualTo(value);

            // Assert
        }

        public static TheoryData<int[], int[], string> NotEqualNullData =>
            new()
            {
                { TestData.Empty,                     null,                 $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { TestData.Single,                    TestData.Empty,       $"Actual collection has more items.{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { TestData.Empty,                     TestData.Single,      $"Actual collection has less items.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { TestData.SingleNotEqual,            TestData.Single,      $"Collections differ at index 0.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.SingleNotEqual.ToFriendlyString()}" },
                { TestData.Multiple,                  TestData.Single,      $"Collections differ at index 0.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { TestData.MultipleNotEqualFirst,     TestData.Multiple,    $"Collections differ at index 0.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { TestData.MultipleNotEqualMiddle,    TestData.Multiple,    $"Collections differ at index 2.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { TestData.MultipleNotEqualLast,      TestData.Multiple,    $"Collections differ at index 4.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(int[] actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.AsSpan().Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int[], int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}
