using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ReferenceTypeAssertionsTests
    {
        [Fact]
        public void BeNull_With_Null_Should_NotThrow()
        {
            // Arrange
            var actual = (object)null;

            // Act
            actual.Must().BeNull();

            // Assert
        }

        [Fact]
        public void BeNull_With_NotNull_Should_Throw()
        {
            // Arrange
            var actual = new object();

            // Act
            void action() => actual.Must().BeNull();

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<object, object>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Null(exception.Expected);
            Assert.Equal("Expected '<null>' to be equivalent to 'System.Object' but it's not.", exception.Message);
        }
    }
}
