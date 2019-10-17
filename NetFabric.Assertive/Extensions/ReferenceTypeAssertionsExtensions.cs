using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class ReferenceTypeAssertionsExtensions
    {
        public static TAssertions BeNull<TAssertions, TActual>(this TAssertions assertions)
            where TAssertions : ReferenceTypeAssertions<TActual>
            where TActual : class
        {
            if (assertions.Actual is object)
                throw new EqualToAssertionException<TActual, TActual>(assertions.Actual, null);

            return assertions;
        }

        public static TAssertions BeNotNull<TAssertions, TActual>(this TAssertions assertions)
            where TAssertions : ReferenceTypeAssertions<TActual>
            where TActual : class
        {
            if (assertions.Actual is null)
                throw new NotEqualToAssertionException<TActual, TActual>(assertions.Actual, null);

            return assertions;
        }

        public static TAssertions BeSameAs<TAssertions, TActual, TExpected>(this TAssertions assertions, TExpected expected)
            where TAssertions : ReferenceTypeAssertions<TActual>
            where TActual : class
        {
            if (!Object.ReferenceEquals(assertions.Actual, expected))
                throw new ExpectedAssertionException<TActual, TExpected>(assertions.Actual, expected,
                    $"Not the same instance.");

            return assertions;
        }

        public static TAssertions BeNotSameAs<TAssertions, TActual, TExpected>(this TAssertions assertions, TExpected expected)
            where TAssertions : ReferenceTypeAssertions<TActual>
            where TActual : class
        {
            if (Object.ReferenceEquals(assertions.Actual, expected))
                throw new ExpectedAssertionException<TActual, TExpected>(assertions.Actual, expected,
                    $"Same instance.");

            return assertions;
        }
    }
}