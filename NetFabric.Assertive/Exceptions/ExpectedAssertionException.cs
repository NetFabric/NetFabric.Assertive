using System;

namespace NetFabric.Assertive
{
    public class ExpectedAssertionException<TActual, TExpected>
        : ActualAssertionException<TActual>
    {
        public ExpectedAssertionException(TActual actual, TExpected expected, string message)
            : base(actual,
                  $"{message}{Environment.NewLine}Expected: {expected.ToFriendlyString()}") 
            => Expected = expected;

        public TExpected Expected { get; }
    }
}