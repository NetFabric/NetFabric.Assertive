using System;

namespace NetFabric.Assertive
{
    public class EnumerableAssertionException<TActual, TActualItem, TExpected>
        : EqualToAssertionException<EnumerableWrapper<TActual, TActualItem>, TExpected>
    {
        public EnumerableAssertionException(EnumerableWrapper<TActual, TActualItem> actual, TExpected expected)
            : this(actual, expected, $"Expected collections to have same count value.")
        {
        }

        public EnumerableAssertionException(EnumerableWrapper<TActual, TActualItem> actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}