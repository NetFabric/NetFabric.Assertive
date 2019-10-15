using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class NullableValueTypeAssertionsTests
    {
        [Fact]
        public void NotHaveValue_With_Value_Should_NotThrow()
        {
            // Arrange
            var actual = (int?)null;

            // Act
            actual.Must().NotHaveValue();

            // Assert
        }

        [Fact]
        public void NotHaveValue_With_NoValue_Should_Throw()
        {
            // Arrange
            var actual = (int?)0;

            // Act
            void action() => actual.Must().NotHaveValue();

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int?, int?>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Null(exception.Expected);
            Assert.Equal("Expected '<null>' to be equivalent to '0' but it's not.", exception.Message);
        }
    }
}
