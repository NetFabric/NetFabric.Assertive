using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    public sealed class AsyncEnumerableWrapper<T> : IAsyncEnumerable<T>
    {
        readonly EnumerableInfo info;

        internal AsyncEnumerableWrapper(object actual, EnumerableInfo info)
        {
            Actual = actual;
            this.info = info;
        }

        public object Actual { get; }
        public Type DeclaringType => info.GetEnumerator.DeclaringType;

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token = default) 
            => new Enumerator(this);

        public sealed class Enumerator : IAsyncEnumerator<T>
        {
            readonly EnumerableInfo info;
            readonly CancellationToken token;
            readonly object enumerator;

            public Enumerator(AsyncEnumerableWrapper<T> enumerable)
            {
                info = enumerable.info;
                enumerator = info.GetEnumerator.Invoke(enumerable.Actual, Array.Empty<object>());
            }

            public T Current => (T)info.Current.GetValue(enumerator);

            public ValueTask<bool> MoveNextAsync() => (ValueTask<bool>)info.MoveNext.Invoke(enumerator, Array.Empty<object>());

            public ValueTask DisposeAsync() => (ValueTask)info.Dispose?.Invoke(enumerator, Array.Empty<object>());
        }
    }
}