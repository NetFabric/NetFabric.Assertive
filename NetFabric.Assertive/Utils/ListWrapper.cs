using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class ListWrapper<TActualItem> 
        : IEnumerable<TActualItem>
    {
        internal ListWrapper(IList<TActualItem> actual) 
            => Actual = actual;

        public IList<TActualItem> Actual { get; }

        public IEnumerator<TActualItem> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        sealed class Enumerator 
            : IEnumerator<TActualItem>
        {
            readonly IList<TActualItem> actual;
            int index;

            public Enumerator(ListWrapper<TActualItem> enumerable)
            {
                actual = enumerable.Actual;
                index = -1;
                Current = default!;
            }

            [AllowNull]
            public TActualItem Current { get; private set; }
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