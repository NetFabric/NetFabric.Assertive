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
        internal ReferenceTypeAssertionsBase(TActual? actual) 
            => Actual = actual;

        public TActual? Actual { get; }

        protected static TAssertions BeNull<TAssertions>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
            => assertions.Actual switch
            {
                null => assertions,
                _ => throw new EqualToAssertionException<TActual, TActual>(assertions.Actual, null),
            };

        protected static TAssertions BeNotNull<TAssertions>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual>
            => assertions.Actual switch
            {
                null => throw new NotEqualToAssertionException<TActual, TActual>(assertions.Actual, null),
                _ => assertions,
            };

        protected static TAssertions BeSameAs<TAssertions, TOther>(TAssertions assertions, TOther? other)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => ReferenceEquals(assertions.Actual, other)
                ? assertions
                : throw new ExpectedAssertionException<TActual, TOther>(assertions.Actual, other, $"Not the same instance.");

        protected static TAssertions BeNotSameAs<TAssertions, TOther>(TAssertions assertions, TOther? other)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => ReferenceEquals(assertions.Actual, other)
                ? throw new ExpectedAssertionException<TActual, TOther>(assertions.Actual, other, $"Same instance.")
                : assertions;

        protected static TAssertions EvaluateTrue<TAssertions>(TAssertions assertions, Func<TActual?, bool> func)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => func(assertions.Actual)
                ? assertions
                : throw new ActualAssertionException<TActual>(assertions.Actual, $"Evaluates to 'false'.");

        protected static TAssertions EvaluateFalse<TAssertions>(TAssertions assertions, Func<TActual?, bool> func)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => func(assertions.Actual)
                ? throw new ActualAssertionException<TActual>(assertions.Actual, $"Evaluates to 'true'.")
                : assertions;

        protected static TAssertions BeOfType<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => typeof(TActual) == typeof(TType)
                ? assertions
                : throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' to be of type '{typeof(TType)}' but it's not.");

        protected static TAssertions NotBeOfType<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => typeof(TActual) == typeof(TType)
                ? throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' not to be of type '{typeof(TType)}' but it is.")
                : assertions;

        protected static TAssertions BeAssignableTo<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => typeof(TActual).IsAssignableTo(typeof(TType))
                ? assertions
                : throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' to be assignable to '{typeof(TType)}' but it's not.");

        protected static TAssertions BeNotAssignableTo<TAssertions, TType>(TAssertions assertions)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => typeof(TActual).IsAssignableTo(typeof(TType))
                ? throw new ActualAssertionException<TActual>(assertions.Actual, $"Expected '{ObjectExtensions.ToFriendlyString(assertions.Actual)}' to be not assignable to '{typeof(TType)}' but it is.")
                : assertions;

        protected static TAssertions BeEqualTo<TAssertions>(TAssertions assertions, TActual? expected)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => EqualityComparer<TActual>.Default.Equals(assertions.Actual!, expected!)
                ? assertions
                : throw new EqualToAssertionException<TActual, TActual>(assertions.Actual, expected);

        protected static TAssertions BeEqualTo<TAssertions, TExpected>(TAssertions assertions, TExpected? expected, Func<TActual?, TExpected?, bool> comparer)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => comparer(assertions.Actual!, expected!)
                ? assertions
                : throw new EqualToAssertionException<TActual, TExpected>(assertions.Actual, expected);

        protected static TAssertions BeNotEqualTo<TAssertions>(TAssertions assertions, TActual? expected)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => EqualityComparer<TActual>.Default.Equals(assertions.Actual!, expected!)
                ? throw new NotEqualToAssertionException<TActual, TActual>(assertions.Actual, expected)
                : assertions;

        protected static TAssertions BeNotEqualTo<TAssertions, TExpected>(TAssertions assertions, TExpected? expected, Func<TActual?, TExpected?, bool> comparer)
            where TAssertions : ReferenceTypeAssertionsBase<TActual> 
            => comparer(assertions.Actual!, expected!)
                ? throw new NotEqualToAssertionException<TActual, TExpected>(assertions.Actual, expected)
                : assertions;
    }
}