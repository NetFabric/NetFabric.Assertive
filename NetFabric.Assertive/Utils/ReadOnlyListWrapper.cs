using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class ReadOnlyListWrapper<TActualItem> 
        : IEnumerable<TActualItem>
    {
        internal ReadOnlyListWrapper(IReadOnlyList<TActualItem> actual) 
            => Actual = actual;

        public IReadOnlyList<TActualItem> Actual { get; }

        public IEnumerator<TActualItem> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        sealed class Enumerator 
            : IEnumerator<TActualItem>
        {
            readonly IReadOnlyList<TActualItem> actual;
            int index;

            public Enumerator(ReadOnlyListWrapper<TActualItem> enumerable)
            {
                actual = enumerable.Actual;
                index = -1;
                Current = default!;
            }

            public TActualItem? Current { get; private set; }
            TActualItem IEnumerator<TActualItem>.Current => Current!;
            object? IEnumerator.Current => Current;

            public bool MoveNext()
            {
                try
                {
                    Current = actual[++index];
                }
                catch (NotSupportedException)
                {
                    throw;
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