using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ReferenceTypeAssertionsTests
    {
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
        [InlineData(null, 0, "Expected '0' but found '<null>'.")]
        [InlineData(0, null, "Expected '<null>' but found '0'.")]
        [InlineData(0, 1, "Expected '1' but found '0'.")]
        public void BeEqualTo_With_NotEqual_Should_Throw(object actual, object expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<object, object>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(exception.Message, message);
        }
    }
}
