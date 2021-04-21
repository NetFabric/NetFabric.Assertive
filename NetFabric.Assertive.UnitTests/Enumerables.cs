using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
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

        public Enumerator GetEnumerator() => new(this);

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