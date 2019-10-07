using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public class ReferenceTypeAssertionsTests
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
            Assert.Equal(exception.Actual, actual);
            Assert.Equal(exception.Expected, null);
            Assert.Equal(exception.Message, "Expected <null> but found System.Object.");
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

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        public void BeEqualTo_With_Equal_Should_NotThrow(object value)
        {
            // Arrange

            // Act
            value.Must().BeEqualTo(value);

            // Assert
        }

        [Theory]
        [InlineData(null, 0, "Expected 0 but found <null>.")]
        [InlineData(0, null, "Expected <null> but found 0.")]
        [InlineData(0, 1, "Expected 1 but found 0.")]
        public void BeEqualTo_With_NotEqual_Should_Throw(object actual, object expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<object, object>>(action);
            Assert.Equal(exception.Actual, actual);
            Assert.Equal(exception.Expected, expected);
            Assert.Equal(exception.Message, message);
        }
    }
}
