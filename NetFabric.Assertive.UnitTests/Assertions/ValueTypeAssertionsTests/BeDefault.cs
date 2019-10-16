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

        [Theory]
        [InlineData(1, "Expected '0' to be equivalent to '1' but it's not.")]
        public void BeDefault_With_NotDefault_Should_Throw(int actual, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeDefault();

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int, int>>(action);
            Assert.Equal(exception.Actual, actual);
            Assert.Equal(exception.Expected, default);
            Assert.Equal(message, exception.Message);
        }
    }
}
