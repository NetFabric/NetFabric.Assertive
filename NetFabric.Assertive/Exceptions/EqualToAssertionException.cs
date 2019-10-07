using System;
using System.Text;

namespace NetFabric.Assertive
{
    public class EqualToAssertionException<TActual, TExpected>
        : ExpectedAssertionException<TActual, TExpected>
    {
        public EqualToAssertionException(TActual actual, TExpected expected)
            : base(actual, expected, $"Expected {expected.ToFriendlyString()} but found {actual.ToFriendlyString()}.")
        {
        }
    }
}