using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ValueTypeAssertionsTests
    {
        [Theory]
        [InlineData(0)]
        public void BeEqualTo_With_Equal_Should_NotThrow(int value)
        {
            // Arrange

            // Act
            value.Must().BeEqualTo(value);

            // Assert
        }

        public static TheoryData<int, int, string> NotEqualtData =>
            new TheoryData<int, int, string>
            {
                { 0, 1, $"Expected to be equal but it's not.{Environment.NewLine}Expected: 1{Environment.NewLine}Actual: 0" },
            };

        [Theory]
        [MemberData(nameof(NotEqualtData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(int actual, int expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int, int>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}
