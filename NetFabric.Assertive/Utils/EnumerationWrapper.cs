using System;
using System.Collections;

namespace NetFabric.Assertive
{
    public sealed class EnumerableWrapper : IEnumerable
    {
        readonly EnumerableInfo info;

        internal EnumerableWrapper(object actual, EnumerableInfo info)
        {
            Actual = actual;
            this.info = info;
        }

        public object Actual { get; }
        public Type DeclaringType => info.GetEnumerator.DeclaringType;

        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        public sealed class Enumerator : IEnumerator, IDisposable
        {
            readonly EnumerableInfo info;
            readonly object enumerator;

            public Enumerator(EnumerableWrapper enumerable)
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
}