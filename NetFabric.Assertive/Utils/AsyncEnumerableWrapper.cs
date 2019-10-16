using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    public sealed class AsyncEnumerableWrapper<TActual, TActualItem> : IAsyncEnumerable<TActualItem>
    {
        readonly EnumerableInfo info;

        internal AsyncEnumerableWrapper(TActual actual, EnumerableInfo info)
        {
            Actual = actual;
            this.info = info;
        }

        public TActual Actual { get; }
        public Type DeclaringType => info.GetEnumerator.DeclaringType;

        public IAsyncEnumerator<TActualItem> GetAsyncEnumerator(CancellationToken token = default) 
            => new Enumerator(this);

        public sealed class Enumerator : IAsyncEnumerator<TActualItem>
        {
            readonly EnumerableInfo info;
            readonly CancellationToken token;
            readonly object enumerator;

            public Enumerator(AsyncEnumerableWrapper<TActual, TActualItem> enumerable)
            {
                info = enumerable.info;
                enumerator = info.GetEnumerator.Invoke(enumerable.Actual, Array.Empty<object>());
            }

            public TActualItem Current => (TActualItem)info.Current.GetValue(enumerator);

            public ValueTask<bool> MoveNextAsync() => (ValueTask<bool>)info.MoveNext.Invoke(enumerator, Array.Empty<object>());

            public ValueTask DisposeAsync() => (ValueTask)info.Dispose?.Invoke(enumerator, Array.Empty<object>());
        }
    }
}