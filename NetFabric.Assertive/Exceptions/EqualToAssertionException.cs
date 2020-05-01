using System;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    public class EqualToAssertionException<TActual, TExpected>
        : ExpectedAssertionException<TActual, TExpected>
    {
        public EqualToAssertionException([AllowNull]TActual actual, [AllowNull]TExpected expected)
            : this(actual, expected, $"Expected to be equal but it's not.")
        {
        }

        public EqualToAssertionException([AllowNull]TActual actual, [AllowNull]TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}