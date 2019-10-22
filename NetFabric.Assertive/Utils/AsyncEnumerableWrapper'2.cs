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
        readonly EnumerableInfo info;

        public AsyncEnumerableWrapper(TActual actual, EnumerableInfo info)
        {
            Actual = actual;
            this.info = info;
        }

        public TActual Actual { get; }
        public Type DeclaringType 
            => info.GetEnumerator.DeclaringType;

        public IEnumerator<TActualItem> GetEnumerator() 
            => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator()
            => new Enumerator(this);

        public sealed class Enumerator 
            : IEnumerator<TActualItem>
        {
            static readonly object[] CancellationToken = new object[] { default(CancellationToken) };

            readonly EnumerableInfo info;
            readonly object enumerator;

            public Enumerator(AsyncEnumerableWrapper<TActual, TActualItem> enumerable)
            {
                info = enumerable.info;
                enumerator = info.GetEnumerator.GetParameters().Length switch
                {
                    0 => info.GetEnumerator.Invoke(enumerable.Actual, Array.Empty<object>()),

                    1 => info.GetEnumerator.Invoke(enumerable.Actual, CancellationToken),

                    _ => throw new Exception("Unexpected number of parameters for 'GetAsyncEnumerator'."),
                };
            }

            public TActualItem Current 
                => (TActualItem)info.Current.GetValue(enumerator);
            object IEnumerator.Current
                => info.Current.GetValue(enumerator);

            public bool MoveNext()
                => ((ValueTask<bool>)info.MoveNext.Invoke(enumerator, Array.Empty<object>())).GetAwaiter().GetResult();

            public void Reset()
                => throw new NotSupportedException();

            public void Dispose()
            {
                if (info.Dispose is object) 
                    ((ValueTask)info.Dispose.Invoke(enumerator, Array.Empty<object>())).GetAwaiter().GetResult();
            }
        }
    }
}