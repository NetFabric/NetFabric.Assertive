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
            _ = Assert.IsType<BooleanAssertions>(result);
        }

        [Fact]
        public void Must_With_Action_Should_Return_ActionAssertions()
        {
            // Arrange
            Action action = () => { };

            // Act
            var result = action.Must();

            // Assert
            _ = Assert.IsType<ActionAssertions>(result);
        }
    }
}
