using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class ValueTypeAssertionsBase<TActual> 
        : AssertionsBase
        where TActual : struct
    {
        internal ValueTypeAssertionsBase(TActual actual)
        {
            Actual = actual;
        }

        public TActual Actual { get; }

        protected static TAssertions EvaluateTrue<TAssertions>(TAssertions assertions, Func<TActual, bool> func)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (!func(assertions.Actual))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Evaluates to 'false'.");

            return assertions;
        }

        protected static TAssertions EvaluateFalse<TAssertions>(TAssertions assertions, Func<TActual, bool> func)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (func(assertions.Actual))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Evaluates to 'true'.");

            return assertions;
        }

        protected static TAssertions BeOfType<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (typeof(TActual) != typeof(TType))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' to be of type '{typeof(TType)}' but it's not.");

            return assertions;
        }

        protected static TAssertions NotBeOfType<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (typeof(TActual) == typeof(TType))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' not to be of type '{typeof(TType)}' but it is.");

            return assertions;
        }

        protected static TAssertions BeAssignableTo<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (!typeof(TActual).IsAssignableTo(typeof(TType)))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' to be assignable to '{typeof(TType)}' but it's not.");

            return assertions;
        }

        protected static TAssertions BeNotAssignableTo<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (typeof(TActual).IsAssignableTo(typeof(TType)))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' to be not assignable to '{typeof(TType)}' but it is.");

            return assertions;
        }

        protected static TAssertions BeEqualTo<TAssertions>(TAssertions assertions, TActual expected)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (!EqualityComparer<TActual>.Default.Equals(assertions.Actual, expected))
                throw new EqualToAssertionException<TActual, TActual>(assertions.Actual, expected);

            return assertions;
        }

        protected static TAssertions BeEqualTo<TAssertions, TExpected>(TAssertions assertions, TExpected expected, Func<TActual, TExpected, bool> comparer)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (!comparer(assertions.Actual, expected))
                throw new EqualToAssertionException<TActual, TExpected>(assertions.Actual, expected);

            return assertions;
        }

        protected static TAssertions BeNotEqualTo<TAssertions>(TAssertions assertions, TActual expected)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (EqualityComparer<TActual>.Default.Equals(assertions.Actual, expected))
                throw new NotEqualToAssertionException<TActual, TActual>(assertions.Actual, expected);

            return assertions;
        }

        protected static TAssertions BeNotEqualTo<TAssertions, TExpected>(TAssertions assertions, TExpected expected, Func<TActual, TExpected, bool> comparer)
            where TAssertions : ValueTypeAssertionsBase<TActual>
        {
            if (comparer(assertions.Actual, expected))
                throw new NotEqualToAssertionException<TActual, TExpected>(assertions.Actual, expected);

            return assertions;
        }

    }
}