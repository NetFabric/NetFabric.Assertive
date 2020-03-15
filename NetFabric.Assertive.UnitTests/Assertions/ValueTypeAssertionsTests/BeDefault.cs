using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ValueTypeAssertionsTests
    {
        [Theory]
        [InlineData(default(int))]
        public void BeDefault_With_Default_Should_NotThrow(int value)
        {
            // Arrange

            // Act
            value.Must().BeDefault();

            // Assert
        }

        public static TheoryData<int, string> NotDefaultData =>
            new TheoryData<int, string>
            {
                { 1, $"Expected to be equal but it's not.{Environment.NewLine}Expected: 0{Environment.NewLine}Actual: 1" },
            };

        [Theory]
        [MemberData(nameof(NotDefaultData))]
        public void BeDefault_With_NotDefault_Should_Throw(int actual, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeDefault();

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int, int>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(default, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}
