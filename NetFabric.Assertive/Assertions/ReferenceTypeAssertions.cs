using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReferenceTypeAssertions<TActual> 
        : AssertionsBase<TActual>
        where TActual : class
    {
        internal ReferenceTypeAssertions(TActual actual) 
            : base(actual)
        {
        }

        public ReferenceTypeAssertions<TActual> EvaluatesTrue(Func<TActual, bool> func)
        {
            if (!func(Actual))
                throw new ActualAssertionException<TActual>(Actual,
                    $"Evaluates to 'false'.");

            return this;
        }

        public ReferenceTypeAssertions<TActual> EvaluatesFalse(Func<TActual, bool> func)
        {
            if (func(Actual))
                throw new ActualAssertionException<TActual>(Actual,
                    $"Evaluates to 'true'.");

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeOfType<TType>()
        {
            if (typeof(TActual) != typeof(TType))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{Actual.ToFriendlyString()}' to be of type '{typeof(TType)}' but it's not.");

            return this;
        }

        public ReferenceTypeAssertions<TActual> NotBeOfType<TType>()
        {
            if (typeof(TActual) == typeof(TType))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{Actual.ToFriendlyString()}' not to be of type '{typeof(TType)}' but it is.");

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeAssignableTo<TType>()
        {
            if (!typeof(TType).IsAssignableFrom(typeof(TActual)))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{Actual.ToFriendlyString()}' to be assignable to '{typeof(TType)}' but it's not.");

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeNotAssignableTo<TType>()
        {
            if (typeof(TType).IsAssignableFrom(typeof(TActual)))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{Actual.ToFriendlyString()}' to be not assignable to '{typeof(TType)}' but it is.");

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeNull() 
        {
            if (Actual is object)
                throw new EqualToAssertionException<TActual, TActual>(Actual, null);
                
            return this;
        }

        public ReferenceTypeAssertions<TActual> BeNotNull() 
        {
            if (Actual is null)
                throw new NotEqualToAssertionException<TActual, TActual>(Actual, null);

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeEqualTo(TActual expected)
        {
            if (!EqualityComparer<TActual>.Default.Equals(Actual, expected))
                throw new EqualToAssertionException<TActual, TActual>(Actual, expected);

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
        {
            if (!comparer(Actual, expected))
                throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeNotEqualTo(TActual expected)
        {
            if (EqualityComparer<TActual>.Default.Equals(Actual, expected))
                throw new NotEqualToAssertionException<TActual, TActual>(Actual, expected);

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
        {
            if (comparer(Actual, expected))
                throw new NotEqualToAssertionException<TActual, TExpected>(Actual, expected);

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeSameAs<TExpected>(TExpected expected)
        {
            if (!Object.ReferenceEquals(Actual, expected))
                throw new ExpectedAssertionException<TActual, TExpected>(Actual, expected, 
                    $"Expected '{Actual.ToFriendlyString()}' to be same as '{expected.ToFriendlyString()}' but it's not.");

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeNotSameAs<TExpected>(TExpected expected)
        {
            if (Object.ReferenceEquals(Actual, expected))
                throw new ExpectedAssertionException<TActual, TExpected>(Actual, expected,
                    $"Expected '{Actual.ToFriendlyString()}' to be not same as '{expected.ToFriendlyString()}' but it is.");

            return this;
        }

        public EnumerableReferenceTypeAssertions<TActual, KeyValuePair<TActualKey, TActualItem>> BeDictionary<TActualKey, TActualItem>()
            => BeEnumerable<KeyValuePair<TActualKey, TActualItem>>();

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEnumerable<TActualItem>()
        {
            TypeExtensions.AssertIsEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new EnumerableReferenceTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeAsyncEnumerable<TActualItem>()
        {
            TypeExtensions.AssertIsAsyncEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public ObservableReferenceTypeAssertions<TActual, TActualItem> BeObservable<TActualItem>()
        {
            var actualType = typeof(TActual);
            if (!typeof(IObservable<>).MakeGenericType(typeof(TActualItem)).IsAssignableFrom(actualType))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{actualType}' to be an observable but doesn't implement 'IObservable <{typeof(TActualItem)}>'.");

            return new ObservableReferenceTypeAssertions<TActual, TActualItem>(Actual);
        }
    }
}