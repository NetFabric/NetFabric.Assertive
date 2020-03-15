using System;
using System.Threading.Tasks;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncDelegateAssertionsTests
    {
        [Fact]
        public void Task_Throw_With_Equal_Should_NotAssert()
        {
            // Arrange
#pragma warning disable CS0162 // Unreachable code detected
            Func<Task> actual = async () => 
                {
                    throw new ArgumentException();
                    await Task.FromResult<bool>(true);
                };
#pragma warning restore CS0162 // Unreachable code detected

            // Act
            void action() => actual.Must().Throw<ArgumentException>();

            // Assert
            try
            {
                action();
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void Task_Throw_With_Derived_Should_Assert()
        {
            // Arrange
#pragma warning disable CS0162 // Unreachable code detected
            Func<Task> actual = async () =>
            {
                throw new ArgumentNullException();
                await Task.FromResult<bool>(true);
            };
#pragma warning restore CS0162 // Unreachable code detected

            // Act
            void action() => actual.Must().Throw<ArgumentException>();

            // Assert
            var exception = Assert.Throws<AssertionException>(action);
            Assert.Equal("The exception type is not the expected.", exception.Message);
        }


        [Fact]
        public void Task_Throw_With_NoThrow_Should_Assert()
        {
            // Arrange
            Func<Task> actual = () => Task.FromResult<bool>(true);

            // Act
            void action() => actual.Must().Throw<ArgumentException>();

            // Assert
            var exception = Assert.Throws<AssertionException>(action);
            Assert.Equal("No exception was thrown.", exception.Message);
        }
    }
}