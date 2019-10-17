using System;

namespace NetFabric.Assertive
{
    public class EnumerableAssertionException<TActual, TExpected>
        : EqualToAssertionException<EnumerableWrapper<TActual>, TExpected>
    {
        public EnumerableAssertionException(EnumerableWrapper<TActual> actual, TExpected expected)
            : this(actual, expected, $"Expected collections to have same count value.")
        {
        }

        public EnumerableAssertionException(EnumerableWrapper<TActual> actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}