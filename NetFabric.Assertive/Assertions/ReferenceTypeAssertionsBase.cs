using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReferenceTypeAssertionsBase<TAssertions, TActual>
        : AssertionsBase<TAssertions>
        where TAssertions : ReferenceTypeAssertionsBase<TAssertions, TActual>
        where TActual : class
    {
        internal ReferenceTypeAssertionsBase(TActual? actual)
            => Actual = actual;

        public TActual? Actual { get; }

        public TAssertions BeOfType<TType>()
            => typeof(TActual) == typeof(TType)
                ? (TAssertions)this
                : throw new ActualAssertionException<TActual?>(Actual, $"Expected '{ObjectExtensions.ToFriendlyString(Actual)}' to be of type '{typeof(TType)}' but it's not.");

        public TAssertions NotBeOfType<TType>()
            => typeof(TActual) == typeof(TType)
                ? throw new ActualAssertionException<TActual?>(Actual, $"Expected '{ObjectExtensions.ToFriendlyString(Actual)}' not to be of type '{typeof(TType)}' but it is.")
                : (TAssertions)this;

        public TAssertions BeAssignableTo<TType>()
            => typeof(TActual).IsAssignableTo(typeof(TType))
                ? (TAssertions)this
                : throw new ActualAssertionException<TActual?>(Actual, $"Expected '{ObjectExtensions.ToFriendlyString(Actual)}' to be assignable to '{typeof(TType)}' but it's not.");

        public TAssertions BeNotAssignableTo<TType>()
            => typeof(TActual).IsAssignableTo(typeof(TType))
                ? throw new ActualAssertionException<TActual?>(Actual, $"Expected '{ObjectExtensions.ToFriendlyString(Actual)}' to be not assignable to '{typeof(TType)}' but it is.")
                : (TAssertions)this;

        public TAssertions EvaluateTrue(Func<TActual?, bool> func)
            => func(Actual)
                ? (TAssertions)this
                : throw new ActualAssertionException<TActual?>(Actual, $"Evaluates to 'false'.");

        public TAssertions EvaluateFalse(Func<TActual?, bool> func)
            => func(Actual)
                ? throw new ActualAssertionException<TActual?>(Actual, $"Evaluates to 'true'.")
                : (TAssertions)this;

        public TAssertions BeEqualTo(TActual? expected)
            => EqualityComparer<TActual?>.Default.Equals(Actual, expected)
                ? (TAssertions)this
                : throw new EqualToAssertionException<TActual?, TActual?>(Actual, expected);

        public TAssertions BeEqualTo<TExpected>(TExpected? expected, Func<TActual?, TExpected?, bool> comparer)
            => comparer(Actual, expected)
                ? (TAssertions)this
                : throw new EqualToAssertionException<TActual?, TExpected?>(Actual, expected);

        public TAssertions BeNotEqualTo(TActual? expected)
            => EqualityComparer<TActual?>.Default.Equals(Actual, expected)
                ? throw new NotEqualToAssertionException<TActual?, TActual?>(Actual, expected)
                : (TAssertions)this;

        public TAssertions BeNotEqualTo<TExpected>(TExpected? expected, Func<TActual?, TExpected?, bool> comparer)
            => comparer(Actual, expected)
                ? throw new NotEqualToAssertionException<TActual?, TExpected?>(Actual, expected)
                : (TAssertions)this;

        public TAssertions BeNull()
            => Actual switch
            {
                null => (TAssertions)this,
                _ => throw new EqualToAssertionException<TActual?, TActual?>(Actual, null),
            };

        public TAssertions BeNotNull()
            => Actual switch
            {
                null => throw new NotEqualToAssertionException<TActual?, TActual?>(Actual, null),
                _ => (TAssertions)this,
            };

        public TAssertions BeSameAs<TOther>(TOther? other)
            => ReferenceEquals(Actual, other)
                ? (TAssertions)this
                : throw new ExpectedAssertionException<TActual?, TOther?>(Actual, other, $"Not the same instance.");

        public TAssertions BeNotSameAs<TOther>(TOther? other)
            => ReferenceEquals(Actual, other)
                ? throw new ExpectedAssertionException<TActual?, TOther?>(Actual, other, $"Same instance.")
                : (TAssertions)this;

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEnumerableOf<TActualItem>()
        {
            AssertIsEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new EnumerableReferenceTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeAsyncEnumerableOf<TActualItem>()
        {
            AssertIsAsyncEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }
    }
}