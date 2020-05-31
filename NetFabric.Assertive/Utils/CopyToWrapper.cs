using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class CopyToWrapper<TActualItem> 
        : IEnumerable<TActualItem>
    {
        readonly TActualItem[] array;
        readonly int arrayIndex;

        internal CopyToWrapper(ICollection<TActualItem> actual, int arrayIndex)
        {
            Actual = actual;
            this.arrayIndex = arrayIndex;

            array = new TActualItem[actual.Count + arrayIndex];
            actual.CopyTo(array, arrayIndex);
        }

        public ICollection<TActualItem> Actual { get; }

        public IEnumerator<TActualItem> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        sealed class Enumerator 
            : IEnumerator<TActualItem>
        {
            readonly TActualItem[] array;
            int index;

            public Enumerator(CopyToWrapper<TActualItem> enumerable)
            {
                array = enumerable.array;
                index = enumerable.arrayIndex - 1;
                Current = default!;
            }

            [AllowNull]
            public TActualItem Current { get; private set; }
            object? IEnumerator.Current => Current;

            public bool MoveNext()
            {
                try
                {
                    Current = array[++index];
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