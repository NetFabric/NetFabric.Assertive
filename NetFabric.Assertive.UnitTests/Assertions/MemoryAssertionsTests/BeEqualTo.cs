using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class MemoryAssertionsTests
    {
        public static TheoryData<int[]> EqualData =>
            new TheoryData<int[]>
            {
                { new int[] { } },
                { new int[] { 0 } },
                { new int[] { 0, 1, 2, 3} },
            };

        [Theory]
        [MemberData(nameof(EqualData))]
        public void BeEqualTo_With_Equal_Should_NotThrow(int[] value)
        {
            // Arrange

            // Act
            value.AsMemory().Must().BeEqualTo(value);

            // Assert
        }

        public static TheoryData<int[], int[], string> NotEqualNullData =>
            new TheoryData<int[], int[], string>
            {
                { new int[] { }, null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
                { new int[] { }, new int[] { 1 }, $"Actual memory has less items.{Environment.NewLine}Expected: {{1}}{Environment.NewLine}Actual: {{}}" },
                { new int[] { 1 }, new int[] { }, $"Actual memory has more items.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{1}}" },
                { new int[] { 1, 2, 3 }, new int[] { 1, 5, 3 }, $"Memories differ at index 1.{Environment.NewLine}Expected: {{1, 5, 3}}{Environment.NewLine}Actual: {{1, 2, 3}}" },
            };

        [Theory]
        [MemberData(nameof(NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(int[] actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.AsMemory().Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int[], int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}
