using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ArrayAssertionsTests
    {
        public static TheoryData<int[]> EqualData =>
            new TheoryData<int[]>
            {
                { null },
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
            value.Must().BeEqualTo(value);

            // Assert
        }

        public static TheoryData<int[], int[], string> NotEqualNullData =>
            new TheoryData<int[], int[], string>
            {
                { null, new int[] { }, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: <null>" },
                { new int[] { }, null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
                { new int[] { }, new int[] { 1 }, $"Actual array has less items.{Environment.NewLine}Expected: {{1}}{Environment.NewLine}Actual: {{}}" },
                { new int[] { 1 }, new int[] { }, $"Actual array has more items.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{1}}" },
                { new int[] { 1, 2, 3 }, new int[] { 1, 5, 3 }, $"Arrays differ at index 1.{Environment.NewLine}Expected: {{1, 5, 3}}{Environment.NewLine}Actual: {{1, 2, 3}}" },
            };

        [Theory]
        [MemberData(nameof(NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(int[] actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int[], int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}
