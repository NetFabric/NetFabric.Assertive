using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public class CountArrayTests
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
            var exception = Assert.Throws<NotNullException<object>>(action);
            Assert.Same(exception.Actual, actual);
            Assert.Equal(exception.Message, "Expected to be <null> but found System.Object.");
        }


        [Fact]
        public void BeNotNull_With_Null_Should_Throw()
        {
            // Arrange
            var actual = (object)null;

            // Act
            void action() => actual.Must().BeNotNull();

            // Assert
            var exception = Assert.Throws<NullException>(action);
            Assert.Equal(exception.Message, "Expected not <null> but found <null>.");
        }

        [Fact]
        public void BeNotNull_With_NotNull_Should_NotThrow()
        {
            // Arrange
            var actual = new object();

            // Act
            actual.Must().BeNotNull();

            // Assert
        }
    }
}
