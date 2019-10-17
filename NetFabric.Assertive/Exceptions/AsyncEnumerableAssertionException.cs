using System;

namespace NetFabric.Assertive
{
    public class AsyncEnumerableAssertionException<TActual, TExpected>
        : EqualToAssertionException<AsyncEnumerableWrapper<TActual>, TExpected>
    {
        public AsyncEnumerableAssertionException(AsyncEnumerableWrapper<TActual> actual, TExpected expected)
            : this(actual, expected, $"Expected collections to have same count value.")
        {
        }

        public AsyncEnumerableAssertionException(AsyncEnumerableWrapper<TActual> actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}