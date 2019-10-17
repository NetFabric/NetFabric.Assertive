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
                { null, new int[] { }, "Expected '' to be equivalent to '<null>' but it's not." },
                { new int[] { }, null, "Expected '<null>' to be equivalent to '' but it's not." },
                { new int[] { }, new int[] { 1 }, "Expected '1' but found '' with less items." },
                { new int[] { 1 }, new int[] { }, "Expected '' but found '1' with more items." },
                { new int[] { 1, 2, 3 }, new int[] { 1, 5, 3 }, "Expected '1, 5, 3' but found '1, 2, 3' that differs at index 1." },
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
