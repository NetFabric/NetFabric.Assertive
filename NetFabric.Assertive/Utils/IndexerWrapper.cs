using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class IndexerWrapper<TActual, TActualItem> 
        : IEnumerable<TActualItem>
    {
        readonly PropertyInfo indexer;

        internal IndexerWrapper(TActual actual, PropertyInfo indexer)
        {
            Actual = actual;
            this.indexer = indexer;
        }

        public TActual Actual { get; }

        public IEnumerator<TActualItem> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        sealed class Enumerator 
            : IEnumerator<TActualItem>
        {
            readonly TActual actual;
            readonly PropertyInfo indexer;
            readonly object[] indexArray = new object[1];
            int index;

            public Enumerator(IndexerWrapper<TActual, TActualItem> enumerable)
            {
                actual = enumerable.Actual;
                indexer = enumerable.indexer;
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
                    indexArray[0] = ++index;
                    Current = (TActualItem?)indexer.GetValue(actual, indexArray);
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