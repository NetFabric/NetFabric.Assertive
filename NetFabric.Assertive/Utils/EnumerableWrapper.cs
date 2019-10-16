using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Assertive
{
    public sealed class EnumerableWrapper<TActual> : IEnumerable
    {
        readonly EnumerableInfo info;

        internal EnumerableWrapper(TActual actual, EnumerableInfo info)
        {
            Actual = actual;
            this.info = info;
        }

        public TActual Actual { get; }
        public Type DeclaringType => info.GetEnumerator.DeclaringType;

        public IEnumerator GetEnumerator() => new Enumerator(this);

        public sealed class Enumerator : IEnumerator, IDisposable
        {
            readonly EnumerableInfo info;
            readonly object enumerator;

            public Enumerator(EnumerableWrapper<TActual> enumerable)
            {
                info = enumerable.info;
                enumerator = info.GetEnumerator.Invoke(enumerable.Actual, Array.Empty<object>());
            }

            public object Current => info.Current.GetValue(enumerator);

            public bool MoveNext() => (bool)info.MoveNext.Invoke(enumerator, Array.Empty<object>());

            public void Reset() => throw new NotSupportedException();

            public void Dispose() => info.Dispose?.Invoke(enumerator, Array.Empty<object>());
        }
    }

    public sealed class EnumerableWrapper<TActual, TActualItem> : IEnumerable<TActualItem>
    {
        readonly EnumerableInfo info;

        internal EnumerableWrapper(TActual actual, EnumerableInfo info)
        {
            Actual = actual;
            this.info = info;
        }

        public TActual Actual { get; }
        public Type DeclaringType => info.GetEnumerator.DeclaringType;

        public IEnumerator<TActualItem> GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public sealed class Enumerator : IEnumerator<TActualItem>
        {
            readonly EnumerableInfo info;
            readonly object enumerator;

            public Enumerator(EnumerableWrapper<TActual, TActualItem> enumerable)
            {
                info = enumerable.info;
                enumerator = info.GetEnumerator.Invoke(enumerable.Actual, Array.Empty<object>());
            }

            public TActualItem Current => (TActualItem)info.Current.GetValue(enumerator);
            object IEnumerator.Current => info.Current.GetValue(enumerator);

            public bool MoveNext() => (bool)info.MoveNext.Invoke(enumerator, Array.Empty<object>());

            public void Reset() => throw new NotSupportedException();

            public void Dispose() => info.Dispose?.Invoke(enumerator, Array.Empty<object>());
        }
    }
}