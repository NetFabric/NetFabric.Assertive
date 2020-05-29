using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class MemoryAssertionsTests
    {
        public static TheoryData<int[]> EqualData =>
            new TheoryData<int[]>
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
            _ = value.AsMemory().Must().BeEqualTo(value);

            // Assert
        }

        public static TheoryData<int[], int[], string> NotEqualNullData =>
            new TheoryData<int[], int[], string>
            {
                { TestData.Empty, null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { TestData.Empty, TestData.Single, $"Actual Memory has less items.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { TestData.Single, TestData.Empty, $"Actual Memory has more items.{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { TestData.Single, TestData.SingleNotEqual, $"Memory differ at index 0.{Environment.NewLine}Expected: {TestData.SingleNotEqual.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { TestData.Single, TestData.Multiple, $"Memory differ at index 0.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { TestData.Multiple, TestData.MultipleNotEqualFirst, $"Memory differ at index 0.{Environment.NewLine}Expected: {TestData.MultipleNotEqualFirst.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { TestData.Multiple, TestData.MultipleNotEqualMiddle, $"Memory differ at index 2.{Environment.NewLine}Expected: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { TestData.Multiple, TestData.MultipleNotEqualLast, $"Memory differ at index 4.{Environment.NewLine}Expected: {TestData.MultipleNotEqualLast.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(int[] actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.AsMemory().Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int[], int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}
