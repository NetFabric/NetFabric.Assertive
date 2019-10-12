using System;
using System.Collections;
using System.Collections.Generic;
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
            var exception = Assert.Throws<NullException<object>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Equal("Expected not '<null>' but found '<null>'.", exception.Message);
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
