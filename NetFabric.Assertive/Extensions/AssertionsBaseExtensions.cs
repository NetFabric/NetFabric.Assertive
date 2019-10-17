using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class AssertionsBaseExtensions
    {
        public static TAssertions EvaluatesTrue<TAssertions, TActual>(this TAssertions assertions, Func<TActual, bool> func)
            where TAssertions : AssertionsBase<TActual>
        {
            if (!func(assertions.Actual))
                throw new ActualAssertionException<TActual>(assertions.Actual,
                    $"Evaluates to 'false'.");

            return assertions;
        }

        public static TAssertions EvaluatesFalse<TAssertions, TActual>(this TAssertions assertions, Func<TActual, bool> func)
            where TAssertions : AssertionsBase<TActual>
        {
            if (func(assertions.Actual))
                throw new ActualAssertionException<TActual>(assertions.Actual,
                    $"Evaluates to 'true'.");

            return assertions;
        }

        public static TAssertions BeOfType<TAssertions, TActual, TType>(this TAssertions assertions)
            where TAssertions : AssertionsBase<TActual>
        {
            if (typeof(TActual) != typeof(TType))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{assertions.Actual.ToFriendlyString()}' to be of type '{typeof(TType)}' but it's not.");

            return assertions;
        }

        public static TAssertions NotBeOfType<TAssertions, TActual, TType>(this TAssertions assertions)
            where TAssertions : AssertionsBase<TActual>
        {
            if (typeof(TActual) == typeof(TType))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{assertions.Actual.ToFriendlyString()}' not to be of type '{typeof(TType)}' but it is.");

            return assertions;
        }

        public static TAssertions BeAssignableTo<TAssertions, TActual, TType>(this TAssertions assertions)
            where TAssertions : AssertionsBase<TActual>
        {
            if (!typeof(TActual).IsAssignableTo(typeof(TType)))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{assertions.Actual.ToFriendlyString()}' to be assignable to '{typeof(TType)}' but it's not.");

            return assertions;
        }

        public static TAssertions BeNotAssignableTo<TAssertions, TActual, TType>(this TAssertions assertions)
            where TAssertions : AssertionsBase<TActual>
        {
            if (typeof(TActual).IsAssignableTo(typeof(TType)))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{assertions.Actual.ToFriendlyString()}' to be not assignable to '{typeof(TType)}' but it is.");

            return assertions;
        }

        public static TAssertions BeEqualTo<TAssertions, TActual>(this TAssertions assertions, TActual expected)
            where TAssertions : AssertionsBase<TActual>
        {
            if (!EqualityComparer<TActual>.Default.Equals(assertions.Actual, expected))
                throw new EqualToAssertionException<TActual, TActual>(assertions.Actual, expected);

            return assertions;
        }

        public static TAssertions BeEqualTo<TAssertions, TActual, TExpected>(this TAssertions assertions, TExpected expected, Func<TActual, TExpected, bool> comparer)
            where TAssertions : AssertionsBase<TActual>
        {
            if (!comparer(assertions.Actual, expected))
                throw new EqualToAssertionException<TActual, TExpected>(assertions.Actual, expected);

            return assertions;
        }

        public static TAssertions BeNotEqualTo<TAssertions, TActual>(this TAssertions assertions, TActual expected)
            where TAssertions : AssertionsBase<TActual>
        {
            if (EqualityComparer<TActual>.Default.Equals(assertions.Actual, expected))
                throw new NotEqualToAssertionException<TActual, TActual>(assertions.Actual, expected);

            return assertions;
        }

        public static TAssertions BeNotEqualTo<TAssertions, TActual, TExpected>(this TAssertions assertions, TExpected expected, Func<TActual, TExpected, bool> comparer)
            where TAssertions : AssertionsBase<TActual>
        {
            if (comparer(assertions.Actual, expected))
                throw new NotEqualToAssertionException<TActual, TExpected>(assertions.Actual, expected);

            return assertions;
        }
    }
}