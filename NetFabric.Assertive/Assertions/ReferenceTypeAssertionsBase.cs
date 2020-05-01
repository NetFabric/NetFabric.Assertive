using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReferenceTypeAssertionsBase<TActual> 
        : AssertionsBase
        where TActual : class
    {
        internal ReferenceTypeAssertionsBase([AllowNull]TActual actual) 
        {
            Actual = actual;
        }

        [AllowNull]
        public TActual Actual { get; }

        protected static TAssertions BeNull<TAssertions>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (assertions.Actual is object)
                throw new EqualToAssertionException<TActual, TActual>(assertions.Actual, null);

            return assertions;
        }

        protected static TAssertions BeNotNull<TAssertions>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (assertions.Actual is null)
                throw new NotEqualToAssertionException<TActual, TActual>(assertions.Actual, null);

            return assertions;
        }

        protected static TAssertions BeSameAs<TAssertions, TOther>(TAssertions assertions, TOther other)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (!Object.ReferenceEquals(assertions.Actual, other))
                throw new ExpectedAssertionException<TActual, TOther>(assertions.Actual, other, $"Not the same instance.");

            return assertions;
        }

        protected static TAssertions BeNotSameAs<TAssertions, TOther>(TAssertions assertions, TOther other)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (Object.ReferenceEquals(assertions.Actual, other))
                throw new ExpectedAssertionException<TActual, TOther>(assertions.Actual, other, $"Same instance.");

            return assertions;
        }

        protected static TAssertions EvaluateTrue<TAssertions>(TAssertions assertions, Func<TActual, bool> func)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (!func(assertions.Actual))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Evaluates to 'false'.");

            return assertions;
        }

        protected static TAssertions EvaluateFalse<TAssertions>(TAssertions assertions, Func<TActual, bool> func)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (func(assertions.Actual))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Evaluates to 'true'.");

            return assertions;
        }

        protected static TAssertions BeOfType<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (typeof(TActual) != typeof(TType))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' to be of type '{typeof(TType)}' but it's not.");

            return assertions;
        }

        protected static TAssertions NotBeOfType<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (typeof(TActual) == typeof(TType))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' not to be of type '{typeof(TType)}' but it is.");

            return assertions;
        }

        protected static TAssertions BeAssignableTo<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (!typeof(TActual).IsAssignableTo(typeof(TType)))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' to be assignable to '{typeof(TType)}' but it's not.");

            return assertions;
        }

        protected static TAssertions BeNotAssignableTo<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (typeof(TActual).IsAssignableTo(typeof(TType)))
                throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' to be not assignable to '{typeof(TType)}' but it is.");

            return assertions;
        }

        protected static TAssertions BeEqualTo<TAssertions>(TAssertions assertions, TActual expected)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (!EqualityComparer<TActual>.Default.Equals(assertions.Actual, expected))
                throw new EqualToAssertionException<TActual, TActual>(assertions.Actual, expected);

            return assertions;
        }

        protected static TAssertions BeEqualTo<TAssertions, TExpected>(TAssertions assertions, TExpected expected, Func<TActual, TExpected, bool> comparer)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (!comparer(assertions.Actual, expected))
                throw new EqualToAssertionException<TActual, TExpected>(assertions.Actual, expected);

            return assertions;
        }

        protected static TAssertions BeNotEqualTo<TAssertions>(TAssertions assertions, TActual expected)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (EqualityComparer<TActual>.Default.Equals(assertions.Actual, expected))
                throw new NotEqualToAssertionException<TActual, TActual>(assertions.Actual, expected);

            return assertions;
        }

        protected static TAssertions BeNotEqualTo<TAssertions, TExpected>(TAssertions assertions, TExpected expected, Func<TActual, TExpected, bool> comparer)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
        {
            if (comparer(assertions.Actual, expected))
                throw new NotEqualToAssertionException<TActual, TExpected>(assertions.Actual, expected);

            return assertions;
        }
    }
}