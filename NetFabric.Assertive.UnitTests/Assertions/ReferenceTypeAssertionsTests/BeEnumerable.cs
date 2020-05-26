using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ReferenceTypeAssertionsTests
    {
        [Fact]
        public void BeEnumerable_With_MissingGetEnumerator_Should_Throw()
        {
            // Arrange
            var actual = new MissingGetEnumeratorEnumerable<int>();

            // Act
            void action() => actual.Must().BeEnumerableOf<int>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<MissingGetEnumeratorEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal($"Expected to be an enumerable but it's missing a valid 'GetEnumerator' method.{Environment.NewLine}Actual: NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+MissingGetEnumeratorEnumerable`1[System.Int32]", exception.Message);
        }

        [Fact]
        public void BeEnumerable_With_MissingCurrent_Should_Throw()
        {
            // Arrange
            var actual = new MissingCurrentEnumerable<int>();

            // Act
            void action() => actual.Must().BeEnumerableOf<int>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<MissingCurrentEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal($"Expected to be an enumerator but it's missing a valid 'Current' property.{Environment.NewLine}Actual: NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+MissingCurrentEnumerable`1[System.Int32]", exception.Message);
        }

        [Fact]
        public void BeEnumerable_With_WrongCurrent_Should_Throw()
        {
            // Arrange
            var actual = new EmptyEnumerable<int>();

            // Act
            void action() => actual.Must().BeEnumerableOf<string>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<EmptyEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal($"Expected to be an enumerable of 'System.String' but found an enumerable of 'System.Int32'.{Environment.NewLine}Actual: {{}}", exception.Message);
        }

        [Fact]
        public void BeEnumerable_With_MissingMoveNext_Should_Throw()
        {
            // Arrange
            var actual = new MissingMoveNextEnumerable<int>();

            // Act
            void action() => actual.Must().BeEnumerableOf<int>();

            // Assert
            var exception = Assert.Throws<ActualAssertionException<MissingMoveNextEnumerable<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal($"Expected to be an enumerator but it's missing a valid 'MoveNext' method.{Environment.NewLine}Actual: NetFabric.Assertive.UnitTests.ReferenceTypeAssertionsTests+MissingMoveNextEnumerable`1[System.Int32]", exception.Message);
        }

        [Fact]
        public void BeEnumerable_With_NoInterfaces_Should_NotThrow()
        {
            // Arrange
            var actual = new EmptyEnumerable<int>();

            // Act
            _ = actual.Must().BeEnumerableOf<int>();

            // Assert
        }

        [Fact]
        public void BeEnumerable_With_ExplicitInterfaces_Should_NotThrow()
        {
            // Arrange
            var actual = new EmptyEnumerableExplicitInterfaces<int>();

            // Act
            _ = actual.Must().BeEnumerableOf<int>();

            // Assert
        }

        [Fact]
        public void BeEnumerable_With_ByRefCurrent_Should_NotThrow()
        {
            // Arrange
            var actual = new ByRefEnumerable<int>(new int[] { });

            // Act
            _ = actual.Must().BeEnumerableOf<int>();

            // Assert
        }

        [Fact]
        public void BeEnumerable_With_Array_Should_NotThrow()
        {
            // Arrange
            var actual = new int[] { 0, 1, 2, 3 };

            // Act
            _ = actual.Must().BeEnumerableOf<int>();

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
            void IEnumerator.Reset() => throw new NotSupportedException();
            void IDisposable.Dispose() {}
        }

        class ByRefEnumerable<T>
        {
            readonly T[] source;

            public ByRefEnumerable(T[] source) => this.source = source;

            public Enumerator GetEnumerator() => new Enumerator(this);

            public struct Enumerator
            {
                readonly T[] source;
                int current;

                internal Enumerator(ByRefEnumerable<T> enumerable)
                {
                    source = enumerable.source;
                    current = -1;
                }

                public readonly ref readonly T Current => ref source[current];

                public bool MoveNext() => ++current < source.Length; 
            }
        }
    }
}
