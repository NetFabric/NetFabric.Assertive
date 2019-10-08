using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Assertive.UnitTests
{
    public class RangeEnumerable
    {
        readonly int count;

        public RangeEnumerable(int enumerableCount)
        {
            count = enumerableCount;
        }

        public Enumerator GetEnumerator() => new Enumerator(count);

        public struct Enumerator
        {
            readonly int count;
            int current;

            internal Enumerator(int count)
            {
                this.count = count;
                current = -1;
            }

            public readonly int Current => current;

            public bool MoveNext() => ++current < count;
        }
    }

    public class RangeNonGenericEnumerable : RangeEnumerable, IEnumerable
    {
        readonly int count;

        public RangeNonGenericEnumerable(int enumerableCount, int nonGenericEnumerableCount)
            : base(enumerableCount)
        {
            count = nonGenericEnumerableCount;
        }

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(count);

        new class Enumerator : IEnumerator
        {
            readonly int count;
            int current;

            internal Enumerator(int count)
            {
                this.count = count;
                current = -1;
            }

            public object Current => current;

            public bool MoveNext() => ++current < count;

            public void Reset() => current = -1;

            public void Dispose() {}
        }
    }

    public class RangeGenericEnumerable : RangeNonGenericEnumerable, IEnumerable<int>
    {
        readonly int count;

        public RangeGenericEnumerable(int enumerableCount, int nonGenericEnumerableCount, int genericEnumerableCount)
            : base(enumerableCount, nonGenericEnumerableCount)
        {
            count = genericEnumerableCount;
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => new Enumerator(count);

        new class Enumerator : IEnumerator<int>
        {
            readonly int count;
            int current;

            internal Enumerator(int count)
            {
                this.count = count;
                current = -1;
            }

            public int Current => current;
            object IEnumerator.Current => current;

            public bool MoveNext() => ++current < count;

            public void Reset() => current = -1;

            public void Dispose() {}
        }
    }

    public class RangeReadOnlyCollection : RangeGenericEnumerable, IReadOnlyCollection<int>
    {
        public RangeReadOnlyCollection(int enumerableCount, int nonGenericEnumerableCount, int genericEnumerableCount, int readOnlyCollectionCount)
            : base(enumerableCount, nonGenericEnumerableCount, genericEnumerableCount)
        {
            Count = readOnlyCollectionCount;
        }

        public int Count { get; }
    }

    public class RangeReadOnlyList : RangeReadOnlyCollection, IReadOnlyList<int>
    {
        readonly int count;

        public RangeReadOnlyList(int enumerableCount, int nonGenericEnumerableCount, int genericEnumerableCount, int readOnlyCollectionCount, int readOnlyListCount)
            : base(enumerableCount, nonGenericEnumerableCount, genericEnumerableCount, readOnlyCollectionCount)
        {
            count = readOnlyListCount;
        }

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                    ThrowIndexOutOfRangeException();

                return index;

                static void ThrowIndexOutOfRangeException() => throw new IndexOutOfRangeException();
            }
        }
    }
}