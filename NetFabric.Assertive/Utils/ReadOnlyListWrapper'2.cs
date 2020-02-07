using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class ReadOnlyListWrapper<TActual, TActualItem> 
        : IEnumerable<TActualItem>
        where TActual : IReadOnlyList<TActualItem>
    {
        internal ReadOnlyListWrapper(TActual actual)
        {
            Actual = actual;
        }

        public TActual Actual { get; }

        public IEnumerator<TActualItem> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        sealed class Enumerator 
            : IEnumerator<TActualItem>
        {
            readonly TActual actual;
            int index;
            TActualItem current;

            public Enumerator(ReadOnlyListWrapper<TActual, TActualItem> enumerable)
            {
                actual = enumerable.Actual;
                index = -1;
                current = default;
            }

            public TActualItem Current => current;
            object IEnumerator.Current => current;

            public bool MoveNext()
            {
                try
                {
                    current = actual[++index];
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