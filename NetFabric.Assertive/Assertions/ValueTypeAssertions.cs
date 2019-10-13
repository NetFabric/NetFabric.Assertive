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