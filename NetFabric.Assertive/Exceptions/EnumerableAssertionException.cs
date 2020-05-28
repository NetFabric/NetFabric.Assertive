using System;
using NetFabric.Reflection;

namespace NetFabric.Assertive
{
    public class EnumerableAssertionException<TActual, TActualItem, TExpected>
        : EqualToAssertionException<EnumerableWrapper<TActual, TActualItem>, TExpected>
    {
        public EnumerableAssertionException(EnumerableWrapper<TActual, TActualItem> actual, TExpected expected)
            : this(actual, expected, $"Expected collections to have same items.")
        {
        }

        public EnumerableAssertionException(EnumerableWrapper<TActual, TActualItem> actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}