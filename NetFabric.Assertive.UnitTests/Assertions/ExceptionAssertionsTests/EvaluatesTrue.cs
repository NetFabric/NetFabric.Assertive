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
            Action actual = () => throw new ArgumentNullException("Test");

            // Act
            Action action = () => actual.Must().Throw<ArgumentNullException>().EvaluatesTrue(exception => exception.Message == "Something else");

            // Assert
            var exception = Assert.Throws<ActualAssertionException<ArgumentNullException>>(action);
            Assert.Equal($"Evaluates to 'false'.{Environment.NewLine}Actual: System.ArgumentNullException", exception.Message);
        }
    }
}
