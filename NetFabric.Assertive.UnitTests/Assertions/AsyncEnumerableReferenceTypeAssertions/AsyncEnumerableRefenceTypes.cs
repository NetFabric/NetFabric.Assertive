using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Assertive.UnitTests
{
    public class TestAsyncEnumerableRef
    {
        readonly Memory<int> items;

        public TestAsyncEnumerableRef(Memory<int> items) => this.items = items;

        public Enumerator GetAsyncEnumerator()
            => new Enumerator(items);

        public ref struct Enumerator
        {
            readonly Span<int> items;
            int index;

            internal Enumerator(Memory<int> items)
            {
                this.items = items.Span;
                index = -1;
            }

            public int Current => items[index];

            public ValueTask<bool> MoveNextAsync()
                => new ValueTask<bool>(++index < items.Length);
        }
    }

    public class TestAsyncEnumerable
    {
        readonly int[] items;

        public TestAsyncEnumerable(int[] items) => this.items = items;

        public Enumerator GetAsyncEnumerator() 
            => new Enumerator(items);

        public struct Enumerator
        {
            readonly int[] items;
            int index;

            internal Enumerator(int[] items)
            {
                this.items = items;
                index = -1;
            }

            public int Current => items[index];

            public ValueTask<bool> MoveNextAsync() 
                => new ValueTask<bool>(++index < items.Length);
        }
    }

    public class TestCancellableAsyncEnumerable
    {
        readonly int[] items;

        public TestCancellableAsyncEnumerable(int[] items) => this.items = items;

        public Enumerator GetAsyncEnumerator(CancellationToken token = default) 
            => new Enumerator(items, token);

        public struct Enumerator
        {
            readonly int[] items;
            readonly CancellationToken token;
            int index;

            internal Enumerator(int[] items, CancellationToken token)
            {
                this.items = items;
                this.token = token;
                index = -1;
            }

            public int Current => items[index];

            public ValueTask<bool> MoveNextAsync()
            {
                token.ThrowIfCancellationRequested();
                return new ValueTask<bool>(++index < items.Length);
            }
        }
    }

    public class TestGenericAsyncEnumerable 
        : TestCancellableAsyncEnumerable
        , IAsyncEnumerable<int>
    {
        readonly int[] items;

        public TestGenericAsyncEnumerable(int[] items) 
            : base(items)
            => this.items = items;

        public TestGenericAsyncEnumerable(int[] enumerableItems, int[] genericEnumerableItems)
            : base(enumerableItems) 
            => items = genericEnumerableItems;

        IAsyncEnumerator<int> IAsyncEnumerable<int>.GetAsyncEnumerator(CancellationToken token) 
            => new Enumerator(items, token);

        new class Enumerator 
            : IAsyncEnumerator<int>
        {
            readonly int[] items;
            readonly CancellationToken token;
            int index;

            internal Enumerator(int[] items, CancellationToken token)
            {
                this.items = items;
                this.token = token;
                index = -1;
            }

            public int Current => items[index];

            public ValueTask<bool> MoveNextAsync()
            {
                token.ThrowIfCancellationRequested();
                return new ValueTask<bool>(++index < items.Length);
            }

            public ValueTask DisposeAsync()
                => default;
        }
    }
}