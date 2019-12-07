using NetFabric.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class EnumerableWrapper<TActual, TActualItem> 
        : IEnumerable<TActualItem>
    {
        public EnumerableWrapper(TActual actual, EnumerableInfo info)
        {
            Actual = actual;
            Info = info;
        }

        public TActual Actual { get; }
        public EnumerableInfo Info { get; }

        public IEnumerator<TActualItem> GetEnumerator() 
            => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator()
            => new Enumerator(this);

        public sealed class Enumerator 
            : IEnumerator<TActualItem>
        {
            readonly TActual actual;
            readonly EnumerableInfo info;
            readonly object enumerator;

            public Enumerator(EnumerableWrapper<TActual, TActualItem> enumerable)
            {
                actual = enumerable.Actual;
                info = enumerable.Info;
                enumerator = info.InvokeGetEnumerator(actual);
            }

            public TActualItem Current
                => (TActualItem)info.InvokeCurrent(enumerator);
            object IEnumerator.Current
                => info.InvokeCurrent(enumerator);

            public bool MoveNext()
                => info.InvokeMoveNext(enumerator);

            public void Reset() 
                => throw new NotSupportedException();

            public void Dispose()
                => info.InvokeDispose(enumerator);
        }
    }
}