using System;
using NetFabric.Reflection;

namespace NetFabric.Assertive
{
    public class EnumerableAssertionException<TActual, TExpected>
        : EqualToAssertionException<TActual, TExpected>
    {
        public EnumerableAssertionException(TActual actual, TExpected expected)
            : this(actual, expected, $"Expected collections to have same items.")
        {
        }

        public EnumerableAssertionException(TActual actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}