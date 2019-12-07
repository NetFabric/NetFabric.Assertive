using NetFabric.Reflection;
using System;
using System.Collections;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class EnumerableWrapper<TActual> 
        : IEnumerable
    {
        public EnumerableWrapper(TActual actual, EnumerableInfo info)
        {
            Actual = actual;
            Info = info;
        }

        public TActual Actual { get; }
        public EnumerableInfo Info { get; }

        public IEnumerator GetEnumerator() 
            => new Enumerator(this);

        public sealed class Enumerator 
            : IEnumerator
        {
            readonly TActual actual;
            readonly EnumerableInfo info;
            readonly object enumerator;

            public Enumerator(EnumerableWrapper<TActual> enumerable)
            {
                actual = enumerable.Actual;
                info = enumerable.Info;
                enumerator = info.InvokeGetEnumerator(actual);
            }

            public object Current 
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