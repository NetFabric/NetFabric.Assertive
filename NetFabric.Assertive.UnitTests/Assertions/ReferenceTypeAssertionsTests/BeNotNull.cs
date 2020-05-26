using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ReferenceTypeAssertionsTests
    {
        [Fact]
        public void BeNotNull_With_Null_Should_Throw()
        {
            // Arrange
            var actual = (object)null;

            // Act
            void action() => actual.Must().BeNotNull();

            // Assert
            var exception = Assert.Throws<NotEqualToAssertionException<object, object>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Null(exception.NotExpected);
            Assert.Equal($"Expected to be not equal but it is.{Environment.NewLine}Not Expected: <null>{Environment.NewLine}Actual: <null>", exception.Message);
        }

        [Fact]
        public void BeNotNull_With_NotNull_Should_NotThrow()
        {
            // Arrange
            var actual = new object();

            // Act
            _ = actual.Must().BeNotNull();

            // Assert
        }
    }
}
