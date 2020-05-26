using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Assertive.UnitTests
{
    public class RangeAsyncEnumerable
    {
        readonly int count;

        public RangeAsyncEnumerable(int enumerableCount) => count = enumerableCount;

        public Enumerator GetAsyncEnumerator() 
            => new Enumerator(count);

        public struct Enumerator
        {
            readonly int count;

            internal Enumerator(int count)
            {
                this.count = count;
                Current = -1;
            }

            public int Current { get; private set; } 

            public ValueTask<bool> MoveNextAsync() 
                => new ValueTask<bool>(++Current < count);
        }
    }

    public class CancellableRangeAsyncEnumerable
    {
        readonly int count;

        public CancellableRangeAsyncEnumerable(int enumerableCount) 
            => count = enumerableCount;

        public Enumerator GetAsyncEnumerator(CancellationToken token = default) 
            => new Enumerator(count, token);

        public struct Enumerator
        {
            readonly int count;
            readonly CancellationToken token;

            internal Enumerator(int count, CancellationToken token)
            {
                this.count = count;
                this.token = token;
                Current = -1;
            }

            public int Current { get; private set; }

            public ValueTask<bool> MoveNextAsync()
            {
                token.ThrowIfCancellationRequested();
                return new ValueTask<bool>(++Current < count);
            }
        }
    }

    public class RangeGenericAsyncEnumerable 
        : RangeAsyncEnumerable
        , IAsyncEnumerable<int>
    {
        readonly int count;

        public RangeGenericAsyncEnumerable(int enumerableCount, int genericEnumerableCount)
            : base(enumerableCount) 
            => count = genericEnumerableCount;

        IAsyncEnumerator<int> IAsyncEnumerable<int>.GetAsyncEnumerator(CancellationToken token) 
            => new Enumerator(count, token);

        new class Enumerator 
            : IAsyncEnumerator<int>
        {
            readonly int count;
            readonly CancellationToken token;

            internal Enumerator(int count, CancellationToken token)
            {
                this.count = count;
                this.token = token;
                Current = -1;
            }

            public int Current { get; private set; }

            public ValueTask<bool> MoveNextAsync()
            {
                token.ThrowIfCancellationRequested();
                return new ValueTask<bool>(++Current < count);
            }

            public ValueTask DisposeAsync()
                => default;
        }
    }
}