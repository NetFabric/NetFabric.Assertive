using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class NullableValueTypeAssertionsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public void BeEqualTo_With_Equal_Should_NotThrow(int? value)
        {
            // Arrange

            // Act
            _ = value.Must().BeEqualTo(value);

            // Assert
        }

        public static TheoryData<int?, int?, string> NotEqualData =>
            new TheoryData<int?, int?, string>
            {
                { null, 0, $"Expected to be equal but it's not.{Environment.NewLine}Expected: 0{Environment.NewLine}Actual: <null>" },
                { 0, null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: 0" },
                { 0, 1, $"Expected to be equal but it's not.{Environment.NewLine}Expected: 1{Environment.NewLine}Actual: 0" },
            };

        [Theory]
        [MemberData(nameof(NotEqualData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(int? actual, int? expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int?, int?>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}
