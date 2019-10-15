using System;

namespace NetFabric.Assertive
{
    public class EqualToAssertionException<TActual, TExpected>
        : ExpectedAssertionException<TActual, TExpected>
    {
        public EqualToAssertionException(TActual actual, TExpected expected)
            : this(actual, expected, $"Expected '{expected.ToFriendlyString()}' to be equivalent to '{actual.ToFriendlyString()}' but it's not.")
        {
        }

        public EqualToAssertionException(TActual actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }    
    }
}