using System;
using System.Collections;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public sealed class EnumerableWrapper<TActual> 
        : IEnumerable
    {
        readonly EnumerableInfo info;

        public EnumerableWrapper(TActual actual, EnumerableInfo info)
        {
            Actual = actual;
            this.info = info;
        }

        public TActual Actual { get; }
        public Type DeclaringType 
            => info.GetEnumerator.DeclaringType;

        public IEnumerator GetEnumerator() 
            => new Enumerator(this);

        public sealed class Enumerator 
            : IEnumerator
        {
            readonly EnumerableInfo info;
            readonly object enumerator;

            public Enumerator(EnumerableWrapper<TActual> enumerable)
            {
                info = enumerable.info;
                enumerator = info.GetEnumerator.Invoke(enumerable.Actual, Array.Empty<object>());
            }

            public object Current 
                => info.Current.GetValue(enumerator);

            public bool MoveNext() 
                => (bool)info.MoveNext.Invoke(enumerator, Array.Empty<object>());

            public void Reset() 
                => throw new NotSupportedException();

            public void Dispose() 
                => info.Dispose?.Invoke(enumerator, Array.Empty<object>());
        }
    }
}