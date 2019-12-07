using NetFabric.Reflection;
using System;
using System.Collections;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class AsyncEnumerableWrapper<TActual> 
        : IEnumerable
    {
        public AsyncEnumerableWrapper(TActual actual, EnumerableInfo info)
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

            public Enumerator(AsyncEnumerableWrapper<TActual> enumerable)
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

            public object Current 
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