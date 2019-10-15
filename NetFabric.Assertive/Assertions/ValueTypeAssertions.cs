using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class ValueTypeAssertions<TActual> 
        : AssertionsBase<TActual>
        where TActual : struct
    {
        internal ValueTypeAssertions(TActual actual) 
            : base(actual)
        {
        }

        public ValueTypeAssertions<TActual> EvaluatesTrue(Func<TActual, bool> func)
        {
            if (!func(Actual))
                throw new ActualAssertionException<TActual>(Actual,
                    $"Evaluates to 'false'.");

            return this;
        }

        public ValueTypeAssertions<TActual> BeOfType<TType>(TActual expected)
        {
            if (!typeof(TType).IsAssignableFrom(typeof(TActual)))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{Actual.ToFriendlyString()}' to be of type '{typeof(TType)}' but it's not.");

            return this;
        }

        public ValueTypeAssertions<TActual> NotBeOfType<TType>(TActual expected)
        {
            if (typeof(TType).IsAssignableFrom(typeof(TActual)))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{Actual.ToFriendlyString()}' not to be of type '{typeof(TType)}' but it is.");

            return this;
        }

        public ValueTypeAssertions<TActual> BeEqualTo(TActual expected)
        {
            if (!EqualityComparer<TActual>.Default.Equals(Actual, expected))
                throw new EqualToAssertionException<TActual, TActual>(Actual, expected);

            return this;
        }

        public ValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
        {
            if (!comparer(Actual, expected))
                throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

            return this;
        }

        public ValueTypeAssertions<TActual> BeNotEqualTo(TActual expected)
        {
            if (EqualityComparer<TActual>.Default.Equals(Actual, expected))
                throw new NotEqualToAssertionException<TActual, TActual>(Actual, expected);

            return this;
        }

        public ValueTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
        {
            if (comparer(Actual, expected))
                throw new NotEqualToAssertionException<TActual, TExpected>(Actual, expected);

            return this;
        }

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEnumerable<TActualItem>()
        {
            TypeExtensions.AssertIsEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new EnumerableValueTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public ObservableValueTypeAssertions<TActual, TActualItem> BeObservable<TActualItem>()
        {
            var actualType = typeof(TActual);
            if (!typeof(IObservable<>).MakeGenericType(typeof(TActualItem)).IsAssignableFrom(actualType))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{actualType}' to be an observable but doesn't implement 'IObservable <{typeof(TActualItem)}>'.");

            return new ObservableValueTypeAssertions<TActual, TActualItem>(Actual);
        }
    }
}