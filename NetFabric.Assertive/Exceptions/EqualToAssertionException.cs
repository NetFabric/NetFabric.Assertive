using System;

namespace NetFabric.Assertive
{
    public class EqualToAssertionException<TActual, TExpected>
        : ExpectedAssertionException<TActual, TExpected>
    {
        public EqualToAssertionException(TActual actual, TExpected expected)
            : this(actual, expected, $"Expected '{expected.ToFriendlyString()}' but found '{actual.ToFriendlyString()}'.")
        {
        }

        public EqualToAssertionException(TActual actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }    
    }
}