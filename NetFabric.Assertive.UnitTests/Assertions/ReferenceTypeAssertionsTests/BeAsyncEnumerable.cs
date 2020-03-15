using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ReferenceTypeAssertionsTests
    {
        [Fact]
        public void BeAsyncEnumerable_With_MissingGetEnumerator_Should_Throw()
        {
            // Arrange
            var actual = new MissingGetAsyncEnumeratorAsyncEnumerable<int>();

            // Act
            Action action = () => actual.Must().BeAsyncEnumerableOf<int>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<MissingGetAsyncEnumeratorAsyncEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal($"Expected to be an async enumerable but it's missing a valid 'GetAsyncEnumerator' method.{Environment.NewLine}Actual: NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+MissingGetAsyncEnumeratorAsyncEnumerable`1[System.Int32]", exception.Message);
        }

        [Fact]
        public void BeAsyncEnumerable_With_MissingCurrent_Should_Throw()
        {
            // Arrange
            var actual = new MissingCurrentAsyncEnumerable<int>();

            // Act
            Action action = () => actual.Must().BeAsyncEnumerableOf<int>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<MissingCurrentAsyncEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal($"Expected to be an async enumerator but it's missing a valid 'Current' property.{Environment.NewLine}Actual: NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+MissingCurrentAsyncEnumerable`1[System.Int32]", exception.Message);
        }

        [Fact]
        public void BeAsyncEnumerable_With_WrongCurrent_Should_Throw()
        {
            // Arrange
            var actual = new EmptyAsyncEnumerable<int>();

            // Act
            Action action = () => actual.Must().BeAsyncEnumerableOf<string>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<EmptyAsyncEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal($"Expected to be an async enumerable of 'System.String' but found an enumerable of 'System.Int32'.{Environment.NewLine}Actual: {{}}", exception.Message);
        }

        [Fact]
        public void BeAsyncEnumerable_With_MissingMoveNext_Should_Throw()
        {
            // Arrange
            var actual = new MissingMoveNextAsyncAsyncEnumerable<int>();

            // Act
            Action action = () => actual.Must().BeAsyncEnumerableOf<int>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<MissingMoveNextAsyncAsyncEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal($"Expected to be an async enumerator but it's missing a valid 'MoveNextAsync' method.{Environment.NewLine}Actual: NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+MissingMoveNextAsyncAsyncEnumerable`1[System.Int32]", exception.Message);
        }

        [Fact]
        public void BeAsyncEnumerable_With_NoInterfaces_Should_NotThrow()
        {
            // Arrange
            var actual = new EmptyAsyncEnumerable<int>();

            // Act
            actual.Must().BeAsyncEnumerableOf<int>();

            // Assert
        }

        [Fact]
        public void BeAsyncEnumerable_With_ExplicitInterfaces_Should_NotThrow()
        {
            // Arrange
            var actual = new EmptyAsyncEnumerableExplicitInterfaces<int>();

            // Act
            actual.Must().BeAsyncEnumerableOf<int>();

            // Assert
        }

        [Fact]
        public void BeAsyncEnumerable_With_ByRefCurrent_Should_NotThrow()
        {
            // Arrange
            var actual = new ByRefAsyncEnumerable<int>(new int[] { });

            // Act
            actual.Must().BeAsyncEnumerableOf<int>();

            // Assert
        }

        class MissingGetAsyncEnumeratorAsyncEnumerable<T>
        {
        }

        class MissingCurrentAsyncEnumerable<T>
        {
            public MissingCurrentAsyncEnumerable<T> GetAsyncEnumerator() => this;

            public ValueTask<bool> MoveNextAsync() 
                => new ValueTask<bool>(false);
        }

        class MissingMoveNextAsyncAsyncEnumerable<T>
        {
            public MissingMoveNextAsyncAsyncEnumerable<T> GetAsyncEnumerator() => this;

            public T Current => default;
        }

        class EmptyAsyncEnumerable<T>
        {
            public EmptyAsyncEnumerable<T> GetAsyncEnumerator() => this;

            public T Current => default;

            public ValueTask<bool> MoveNextAsync() 
                => new ValueTask<bool>(false);
        }

        class EmptyAsyncEnumerableExplicitInterfaces<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>
        {
            IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken token) => this;

            T IAsyncEnumerator<T>.Current => default;

            ValueTask<bool> IAsyncEnumerator<T>.MoveNextAsync() 
                => new ValueTask<bool>(false);

            ValueTask IAsyncDisposable.DisposeAsync() 
                => default;
        }

        class ByRefAsyncEnumerable<T>
        {
            readonly T[] source;

            public ByRefAsyncEnumerable(T[] source)
            {
                this.source = source;
            }

            public Enumerator GetAsyncEnumerator() => new Enumerator(this);

            public struct Enumerator
            {
                readonly T[] source;
                int current;

                internal Enumerator(ByRefAsyncEnumerable<T> enumerable)
                {
                    source = enumerable.source;
                    current = -1;
                }

                public readonly ref readonly T Current 
                    => ref source[current];

                public ValueTask<bool> MoveNextAsync() 
                    => new ValueTask<bool>(++current < source.Length);
            }
        }
    }
}
