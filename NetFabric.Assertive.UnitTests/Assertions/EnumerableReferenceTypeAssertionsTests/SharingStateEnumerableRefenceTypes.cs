using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Assertive.UnitTests
{
    public class SharingStateRangeEnumerable
    {
        readonly int count;
		int current;

        public SharingStateRangeEnumerable(int count)
        {
            this.count = count;
			current = -1;
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        public readonly struct Enumerator
        {
            readonly SharingStateRangeEnumerable enumerable;

            internal Enumerator(SharingStateRangeEnumerable enumerable)
            {
                this.enumerable = enumerable;
            }

            public readonly int Current => enumerable.current;

            public bool MoveNext() => ++enumerable.current < enumerable.count;
        }
    }

    public class SharingStateRangeNonGenericEnumerable 
        : RangeEnumerable
        , IEnumerable
    {
        readonly int count;
        int current;

        public SharingStateRangeNonGenericEnumerable(int count)
            : base(count)
        {
            this.count = count;
            current = -1;
        }

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        new class Enumerator : IEnumerator
        {
            readonly SharingStateRangeNonGenericEnumerable enumerable;

            internal Enumerator(SharingStateRangeNonGenericEnumerable enumerable)
            {
                this.enumerable = enumerable;
            }

            public object Current => enumerable.current;

            public bool MoveNext() => ++enumerable.current < enumerable.count;

            public void Reset() => enumerable.current = -1;

            public void Dispose() {}
        }
    }

    public class SharingStateRangeGenericEnumerable 
        : RangeNonGenericEnumerable
        , IEnumerable<int>
    {
        readonly int count;
        int current;

        public SharingStateRangeGenericEnumerable(int count)
            : base(count, count)
        {
            this.count = count;
            current = -1;
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => new Enumerator(this);

        new class Enumerator : IEnumerator<int>
        {
            readonly SharingStateRangeGenericEnumerable enumerable;

            internal Enumerator(SharingStateRangeGenericEnumerable enumerable)
            {
                this.enumerable = enumerable;
            }

            public int Current => enumerable.current;
            object IEnumerator.Current => enumerable.current;

            public bool MoveNext() => ++enumerable.current < enumerable.count;

            public void Reset() => enumerable.current = -1;

            public void Dispose() {}
        }
    }
}