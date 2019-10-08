using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Assertive
{
    sealed class ReadOnlyListWrapper<T> : IEnumerable<T>
    {
        readonly IReadOnlyList<T> source;

        internal ReadOnlyListWrapper(IReadOnlyList<T> source)
        {
            this.source = source;
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public sealed class Enumerator : IEnumerator<T>
        {
            readonly IReadOnlyList<T> source;
            int index;
            T current;

            public Enumerator(ReadOnlyListWrapper<T> enumerable)
            {
                source = enumerable.source;
                index = -1;
                current = default;
            }

            public T Current => current;
            object IEnumerator.Current => current;

            public bool MoveNext()
            {
                try
                {
                    current = source[++index];
                }
                catch
                {
                    return false;
                }

                return true;
            }

            public void Reset() => throw new NotSupportedException();

            public void Dispose() { }
        }
    }
}