using System;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    public class ExpectedAssertionException<TActual, TExpected>
        : ActualAssertionException<TActual>
    {
        public ExpectedAssertionException([AllowNull]TActual actual, [AllowNull]TExpected expected, string message)
            : base(actual, 
                  $"{message}{Environment.NewLine}Expected: {ObjectExtensions.ToFriendlyString(expected)}")
        {
            Expected = expected;
        }

        [AllowNull]
        public TExpected Expected { get; }
    }
}