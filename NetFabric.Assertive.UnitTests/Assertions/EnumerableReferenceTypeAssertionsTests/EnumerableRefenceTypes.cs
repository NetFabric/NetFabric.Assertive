using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Assertive.UnitTests
{
    public class ExceptionOnGetEnumeratorEnumerable<T>
    {
        public ExceptionOnGetEnumeratorEnumerable<T> GetEnumerator() => throw new Exception();
        public T Current => default;
        public bool MoveNext() => false;
    }

    public class ExceptionOnCurrentEnumerable<T>
    {
        public ExceptionOnCurrentEnumerable<T> GetEnumerator() => this;
        public T Current => throw new Exception();
        public bool MoveNext() => true;
    }

    public class ExceptionOnMoveNextEnumerable<T>
    {
        public ExceptionOnMoveNextEnumerable<T> GetEnumerator() => this;
        public T Current => default;
        public bool MoveNext() => throw new Exception();
    }

    public class ExceptionOnDisposeEnumerable<T> : IDisposable
    {
        public ExceptionOnDisposeEnumerable<T> GetEnumerator() => this;
        public T Current => default;
        public bool MoveNext() => false;
        public void Dispose() => throw new Exception();
    }


#if NETCORE
    public class TestEnumerableRef
    {
        readonly Memory<int> items;

        public TestEnumerableRef(Memory<int> items) => this.items = items;

        public Enumerator GetEnumerator() => new Enumerator(items);

        public ref struct Enumerator
        {
            readonly ReadOnlySpan<int> items;
            int index;

            internal Enumerator(Memory<int> items)
            {
                this.items = items.Span;
                index = -1;
            }

            public int Current => items[index];

            public bool MoveNext() => ++index < items.Length;
        }
    }
#endif

    public class TestEnumerable
    {
        readonly int[] items;

        public TestEnumerable(int[] items) => this.items = items;

        public Enumerator GetEnumerator() => new Enumerator(items);

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

            public bool MoveNext() => ++index < items.Length;
        }
    }

    public class TestNonGenericEnumerable : TestEnumerable, IEnumerable
    {
        readonly int[] items;

        public TestNonGenericEnumerable(int[] items)
            : base(items) 
            => this.items = items;

        public TestNonGenericEnumerable(int[] enumerableItems, int[] nonGenericEnumerableItems)
            : base(enumerableItems)
            => items = nonGenericEnumerableItems;

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(items);

        new class Enumerator : IEnumerator
        {
            readonly int[] items;
            int index;

            internal Enumerator(int[] items)
            {
                this.items = items;
                index = -1;
            }

            public object Current => items[index];

            public bool MoveNext() => ++index < items.Length;

            public void Reset() => index = -1;

            public void Dispose() {}
        }
    }

    public class TestGenericEnumerable : TestNonGenericEnumerable, IEnumerable<int>
    {
        readonly int[] items;

        public TestGenericEnumerable(int[] items)
            : base(items)
            => this.items = items;

        public TestGenericEnumerable(int[] nonGenericEnumerableItems, int[] genericEnumerableItems)
            : base(nonGenericEnumerableItems) 
            => items = genericEnumerableItems;

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => new Enumerator(items);

        new class Enumerator : IEnumerator<int>
        {
            readonly int[] items;
            int index;

            internal Enumerator(int[] items)
            {
                this.items = items;
                index = -1;
            }

            public int Current => items[index];
            object IEnumerator.Current => items[index];

            public bool MoveNext() => ++index < items.Length;

            public void Reset() => index = -1;

            public void Dispose() {}
        }
    }

    public class TestCollection : TestGenericEnumerable, IReadOnlyCollection<int>, ICollection<int>
    {
        readonly int[] copyToItems;
        readonly int[] containsItems;

        public TestCollection(int[] items)
            : base(items)
        {
            Count = items.Length;
            copyToItems = items;
            containsItems = items;
        }

        public TestCollection(int[] genericEnumerableItems, int count, int[] copyToItems, int[] containsItems)
            : base(genericEnumerableItems)
        {
            Count = count;
            this.copyToItems = copyToItems;
            this.containsItems = containsItems;
        }

        public int Count { get; }

        bool ICollection<int>.IsReadOnly => true;

        public bool Contains(int item) => containsItems.Contains(item);

        public void CopyTo(int[] array, int arrayIndex)
        {
            for (var index = 0; index < copyToItems.Length; index++)
                array[index + arrayIndex] = copyToItems[index];
        }

        void ICollection<int>.Add(int item) => throw new NotSupportedException();
        void ICollection<int>.Clear() => throw new NotSupportedException();
        bool ICollection<int>.Remove(int item) => throw new NotSupportedException();
    }

    public class TestList : TestCollection, IReadOnlyList<int>, IList<int>
    {
        readonly int[] privateIndexerItems;
        readonly int[] publicIndexerItems;
        readonly int[] indexOfItems;

        public TestList(int[] items)
            : base(items)
        {
            privateIndexerItems = items;
            publicIndexerItems = items;
            indexOfItems = items;
        }

        public TestList(int[] collectionItems, int[] privateIndexerItems, int[] publicIndexerItems, int[] indexOfItems)
            : base(collectionItems)
        {
            this.privateIndexerItems = privateIndexerItems;
            this.publicIndexerItems = publicIndexerItems;
            this.indexOfItems = indexOfItems;
        }

        public int this[int index] => publicIndexerItems[index];

        int IList<int>.this[int index] 
        { 
            get => privateIndexerItems[index]; 
            set => throw new NotSupportedException(); 
        }

        public int IndexOf(int item) => ((IList<int>)indexOfItems).IndexOf(item);

        void IList<int>.Insert(int index, int item) => throw new NotSupportedException();
        void IList<int>.RemoveAt(int index) => throw new NotSupportedException();
    }
}