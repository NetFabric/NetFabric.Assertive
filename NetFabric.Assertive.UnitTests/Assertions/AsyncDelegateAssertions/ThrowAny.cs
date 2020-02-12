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
            Func<Task> actual = async () => 
                {
                    throw new ArgumentException();
                    await Task.FromResult<bool>(true); 
                };

            // Act
            Action action = () => actual.Must().ThrowAny<ArgumentException>();

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
            Func<ValueTask> actual = async () => 
                {
                    throw new ArgumentNullException();
                    await Task.FromResult<bool>(true); 
                };

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
            Func<Task> actual = () => Task.FromResult<bool>(true);

            // Act
            Action action = () => actual.Must().ThrowAny<ArgumentException>();

            // Assert
            var exception = Assert.Throws<AssertionException>(action);
            Assert.Equal("No exception was thrown.", exception.Message);
        }
    }
}