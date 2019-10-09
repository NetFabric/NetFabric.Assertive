using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public class ValueTypeAssertionsTests
    {
        [Theory]
        [InlineData(0)]
        public void BeEqualTo_With_Equal_Should_NotThrow(int value)
        {
            // Arrange

            // Act
            value.Must().BeEqualTo(value);

            // Assert
        }

        [Theory]
        [InlineData(0, 1, "Expected '1' but found '0'.")]
        public void BeEqualTo_With_NotEqual_Should_Throw(int actual, int expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<int, int>>(action);
            Assert.Equal(exception.Actual, actual);
            Assert.Equal(exception.Expected, expected);
            Assert.Equal(message, exception.Message);
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
            Assert.Equal(exception.Actual, actual);
            Assert.Equal(exception.Message, "Expected 'NetFabric.Assertive.UnitTests.ValueTypeAssertionsTests+MissingGetEnumeratorEnumerable`1[System.Int32]' to be an enumerable but it's missing a valid 'GetEnumerator' method.");
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
            Assert.Equal(exception.Actual, actual);
            Assert.Equal(exception.Message, "Expected 'NetFabric.Assertive.UnitTests.ValueTypeAssertionsTests+MissingCurrentEnumerable`1[System.Int32]' to be an enumerator but it's missing a valid 'Current' property.");
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
            Assert.Equal(exception.Actual, actual);
            Assert.Equal(exception.Message, "Expected 'NetFabric.Assertive.UnitTests.ValueTypeAssertionsTests+EmptyEnumerable`1[System.Int32]' to be an enumerable of 'System.String' but found an enumerable of 'System.Int32'.");
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
            Assert.Equal(exception.Actual, actual);
            Assert.Equal(exception.Message, "Expected 'NetFabric.Assertive.UnitTests.ValueTypeAssertionsTests+MissingMoveNextEnumerable`1[System.Int32]' to be an enumerator but it's missing a valid 'MoveNext' method.");
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

        readonly struct MissingGetEnumeratorEnumerable<T>
        {
        }

        readonly struct MissingCurrentEnumerable<T>
        {
            public readonly MissingCurrentEnumerable<T> GetEnumerator() => this;

            public bool MoveNext() => false;
        }

        readonly struct MissingMoveNextEnumerable<T>
        {
            public readonly MissingMoveNextEnumerable<T> GetEnumerator() => this;

            public T Current => default;
        }

        readonly struct EmptyEnumerable<T>
        {
            public readonly EmptyEnumerable<T> GetEnumerator() => this;

            public readonly T Current => default;

            public bool MoveNext() => false;
        }

        readonly struct EmptyEnumerableExplicitInterfaces<T> : IEnumerable<T>, IEnumerator<T>
        {
            readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;
            readonly IEnumerator IEnumerable.GetEnumerator() => this;

            readonly T IEnumerator<T>.Current => default;
            readonly object IEnumerator.Current => default;

            bool IEnumerator.MoveNext() => false;
            void IEnumerator.Reset() {}
            void IDisposable.Dispose() {}
        }
    }
}
