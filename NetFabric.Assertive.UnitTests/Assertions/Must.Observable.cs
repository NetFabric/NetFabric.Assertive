using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class MustTests
    {
        [Fact]
        public void Must_With_Boolean_Should_Return_BooleanAssertions()
        {
            // Arrange
            var actual = false;

            // Act
            var result = actual.Must();

            // Assert
            Assert.IsType<BooleanAssertions>(result);
        }
    }
}
