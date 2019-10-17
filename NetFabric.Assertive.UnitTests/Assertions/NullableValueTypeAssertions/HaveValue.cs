using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class NullableValueTypeAssertionsTests
    {
        [Fact]
        public void HaveValue_With_Value_Should_NotThrow()
        {
            // Arrange
            var actual = (int?)0;

            // Act
            actual.Must().HaveValue();

            // Assert
        }

        [Fact]
        public void HaveValue_With_NoValue_Should_Throw()
        {
            // Arrange
            var actual = (int?)null;

            // Act
            void action() => actual.Must().HaveValue();

            // Assert
            var exception = Assert.Throws<NotEqualToAssertionException<int?, int?>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Null(exception.NotExpected);
            Assert.Equal($"Expected to be not equal but it is.{Environment.NewLine}Not Expected: <null>{Environment.NewLine}Actual: <null>", exception.Message);
        }
    }
}
