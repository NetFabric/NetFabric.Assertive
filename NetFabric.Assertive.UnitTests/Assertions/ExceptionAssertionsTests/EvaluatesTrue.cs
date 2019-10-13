using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ExceptionAssertionsTests
    {
        [Fact]
        public void EvaluatesTrue_With_True_Should_NotThrow()
        {
            // Arrange
            var message = "Test";
            Action action = () => throw new Exception(message);

            // Act
            action.Must().Throw<Exception>().EvaluatesTrue(exception => exception.Message == message);

            // Assert
        }

        [Fact]
        public void EvaluatesTrue_With_False_Should_Throw()
        {
            // Arrange
            Action actual = () => throw new Exception("Test");

            // Act
            void action() => actual.Must().Throw<Exception>().EvaluatesTrue(exception => exception.Message == "Something else");

            // Assert
            var exception = Assert.Throws<ActualAssertionException<Exception>>(action);
            Assert.Equal("Evaluates to 'false'.", exception.Message);
        }
    }
}
