using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public class ReferenceTypeAssertionsTests
    {
        [Fact]
        public void BeNull_With_Null_Should_NotThrow()
        {
            // Arrange
            var actual = (object)null;

            // Act
            actual.Must().BeNull();

            // Assert
        }

        [Fact]
        public void BeNull_With_NotNull_Should_Throw()
        {
            // Arrange
            var actual = new object();

            // Act
            void action() => actual.Must().BeNull();

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<object, object>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(null, exception.Expected);
            Assert.Equal("Expected '<null>' but found 'System.Object'.", exception.Message);
        }


        [Fact]
        public void BeNotNull_With_Null_Should_Throw()
        {
            // Arrange
            var actual = (object)null;

            // Act
            void action() => actual.Must().BeNotNull();

            // Assert
            var exception = Assert.Throws<NullException<object>>(action);
            Assert.Equal("Expected not '<null>' but found '<null>'.", exception.Message);
        }

        [Fact]
        public void BeNotNull_With_NotNull_Should_NotThrow()
        {
            // Arrange
            var actual = new object();

            // Act
            actual.Must().BeNotNull();

            // Assert
        }

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

        [Fact]
        public void BeEnumerable_With_MissingGetEnumerator_Should_Throw()
        {
            // Arrange
            var actual = new MissingGetEnumeratorEnumerable<int>();

            // Act
            void action() => actual.Must().BeEnumerable<int>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<MissingGetEnumeratorEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal("Expected 'NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+MissingGetEnumeratorEnumerable`1[System.Int32]' to be an enumerable but it's missing a valid 'GetEnumerator' method.", exception.Message);
        }

        [Fact]
        public void BeEnumerable_With_MissingCurrent_Should_Throw()
        {
            // Arrange
            var actual = new MissingCurrentEnumerable<int>();

            // Act
            void action() => actual.Must().BeEnumerable<int>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<MissingCurrentEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal("Expected 'NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+MissingCurrentEnumerable`1[System.Int32]' to be an enumerator but it's missing a valid 'Current' property.", exception.Message);
        }

        [Fact]
        public void BeEnumerable_With_WrongCurrent_Should_Throw()
        {
            // Arrange
            var actual = new EmptyEnumerable<int>();

            // Act
            void action() => actual.Must().BeEnumerable<string>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<EmptyEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal("Expected 'NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+EmptyEnumerable`1[System.Int32]' to be an enumerable of 'System.String' but found an enumerable of 'System.Int32'.", exception.Message);
        }

        [Fact]
        public void BeEnumerable_With_MissingMoveNext_Should_Throw()
        {
            // Arrange
            var actual = new MissingMoveNextEnumerable<int>();

            // Act
            void action() => actual.Must().BeEnumerable<int>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<MissingMoveNextEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal("Expected 'NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+MissingMoveNextEnumerable`1[System.Int32]' to be an enumerator but it's missing a valid 'MoveNext' method.", exception.Message);
        }

        [Fact]
        public void BeEnumerable_With_NoInterfaces_Should_NotThrow()
        {
            // Arrange
            var actual = new EmptyEnumerable<int>();

            // Act
            actual.Must().BeEnumerable<int>();

            // Assert
        }

        [Fact]
        public void BeEnumerable_With_ExplicitInterfaces_Should_NotThrow()
        {
            // Arrange
            var actual = new EmptyEnumerableExplicitInterfaces<int>();

            // Act
            actual.Must().BeEnumerable<int>();

            // Assert
        }

        class MissingGetEnumeratorEnumerable<T>
        {
        }

        class MissingCurrentEnumerable<T>
        {
            public MissingCurrentEnumerable<T> GetEnumerator() => this;

            public bool MoveNext() => false;
        }

        class MissingMoveNextEnumerable<T>
        {
            public MissingMoveNextEnumerable<T> GetEnumerator() => this;

            public T Current => default;
        }

        class EmptyEnumerable<T>
        {
            public EmptyEnumerable<T> GetEnumerator() => this;

            public T Current => default;

            public bool MoveNext() => false;
        }

        class EmptyEnumerableExplicitInterfaces<T> : IEnumerable<T>, IEnumerator<T>
        {
            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
            IEnumerator IEnumerable.GetEnumerator() => this;

            T IEnumerator<T>.Current => default;
            object IEnumerator.Current => default;

            bool IEnumerator.MoveNext() => false;
            void IEnumerator.Reset() {}
            void IDisposable.Dispose() {}
        }
    }
}
