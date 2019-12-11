using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class DelegateAssertionsTests
    {
        [Fact]
        public void ThrowAny_With_Equal_Should_NotAssert()
        {
            // Arrange
            Action actual = () => throw new ArgumentException();

            // Act
            Action action = () => actual.Must().ThrowAny<ArgumentException>();

            // Assert
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void ThrowAny_With_Derived_Should_NotAssert()
        {
            // Arrange
            Action actual = () => throw new ArgumentNullException();

            // Act
            Action action = () => actual.Must().ThrowAny<ArgumentException>();

            // Assert
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Assert.True(false);
            }
        }


        [Fact]
        public void ThrowAny_With_NoThrow_Should_Assert()
        {
            // Arrange
            Action actual = () => {};

            // Act
            Action action = () => actual.Must().ThrowAny<ArgumentException>();

            // Assert
            var exception = Assert.Throws<AssertionException>(action);
            Assert.Equal("No exception was thrown.", exception.Message);
        }
    }
}