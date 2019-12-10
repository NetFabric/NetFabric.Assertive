using System;
using NetFabric.Reflection;

namespace NetFabric.Assertive
{
    public class AsyncEnumerableAssertionException<TActual, TActualItem, TExpected>
        : EqualToAssertionException<AsyncEnumerableWrapper<TActual, TActualItem>, TExpected>
    {
        public AsyncEnumerableAssertionException(AsyncEnumerableWrapper<TActual, TActualItem> actual, TExpected expected)
            : this(actual, expected, $"Expected collections to have same count value.")
        {
        }

        public AsyncEnumerableAssertionException(AsyncEnumerableWrapper<TActual, TActualItem> actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}