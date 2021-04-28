using System;
using System.Collections.Concurrent;
using System.Threading;

namespace NetFabric.Assertive
{
    class LazyConcurrentDictionary<TKey, TValue>
    {
        readonly ConcurrentDictionary<TKey, Lazy<TValue>> concurrentDictionary = new();

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            var lazyResult = concurrentDictionary.GetOrAdd(
                key, 
                k => new Lazy<TValue>(() => valueFactory(k), LazyThreadSafetyMode.ExecutionAndPublication));

            return lazyResult.Value;
        }
    }
}