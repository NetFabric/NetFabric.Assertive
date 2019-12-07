using NetFabric.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class AsyncEnumerableWrapper<TActual, TActualItem> 
        : IEnumerable<TActualItem>
    {
        public AsyncEnumerableWrapper(TActual actual, EnumerableInfo info)
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

            public Enumerator(AsyncEnumerableWrapper<TActual, TActualItem> enumerable)
            {
                actual = enumerable.Actual;
                info = enumerable.Info;
                enumerator = info.GetEnumerator.GetParameters().Length switch
                {
                    0 => info.InvokeGetAsyncEnumerator(actual),

                    1 => info.InvokeGetAsyncEnumerator(actual, default),

                    _ => throw new Exception("Unexpected number of parameters for 'GetAsyncEnumerator'."),
                };
            }

            public TActualItem Current
                => (TActualItem)info.InvokeCurrent(enumerator);
            object IEnumerator.Current
                => info.InvokeCurrent(enumerator);

            public bool MoveNext()
                => info.InvokeMoveNextAsync(enumerator);

            public void Reset()
                => throw new NotSupportedException();

            public void Dispose()
                => info.InvokeDispose(enumerator);
        }
    }
}