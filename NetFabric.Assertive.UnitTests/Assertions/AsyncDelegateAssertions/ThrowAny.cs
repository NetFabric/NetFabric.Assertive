using System;
using System.Threading.Tasks;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncDelegateAssertionsTests
    {
        [Fact]
        public void ThrowAny_With_Equal_Should_NotAssert()
        {
            // Arrange
#pragma warning disable CS0162 // Unreachable code detected
            Func<Task> actual = async () => 
                {
                    throw new ArgumentException();
                    _ = await Task.FromResult<bool>(true); 
                };
#pragma warning restore CS0162 // Unreachable code detected

            // Act
            void action() => actual.Must().ThrowAny<ArgumentException>();

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
        public void ThrowAny_With_Derived_Should_NotAssert()
        {
            // Arrange
#pragma warning disable CS0162 // Unreachable code detected
            Func<Task> actual = async () =>
            {
                throw new ArgumentNullException();
                _ = await Task.FromResult<bool>(true);
            };
#pragma warning restore CS0162 // Unreachable code detected

            // Act
            void action() => actual.Must().ThrowAny<ArgumentException>();

            // Assert
            try
            {
                action();
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }


        [Fact]
        public void ThrowAny_With_NoThrow_Should_Assert()
        {
            // Arrange
            Func<Task> actual = () => Task.FromResult<bool>(true);

            // Act
            void action() => actual.Must().ThrowAny<ArgumentException>();

            // Assert
            var exception = Assert.Throws<AssertionException>(action);
            Assert.Equal("No exception was thrown.", exception.Message);
        }
    }
}