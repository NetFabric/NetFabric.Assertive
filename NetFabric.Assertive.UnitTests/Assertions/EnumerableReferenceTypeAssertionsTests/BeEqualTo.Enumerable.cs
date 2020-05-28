using System;
using NetFabric.Reflection;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        [Fact]
        public void Enumerable_BeEqualTo_With_ExceptionOnGetEnumerator_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnGetEnumeratorEnumerable<int>();
            var expected = new int[0];

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionOnGetEnumeratorEnumerable`1.GetEnumerator().", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void BeEqualTo_With_ExceptionOnCurrent_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnCurrentEnumerable<int>();
            var expected = new int[0];

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionOnCurrentEnumerable`1.Current.", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void BeEqualTo_With_ExceptionOnMoveNext_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnMoveNextEnumerable<int>();
            var expected = new int[0];

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionOnMoveNextEnumerable`1.MoveNext().", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void BeEqualTo_With_ExceptionOnDispose_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnDisposeEnumerable<int>();
            var expected = new int[0];

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in IDisposable.Dispose().", exception.Message);
            Assert.NotNull(exception.InnerException);
        }
    }
}