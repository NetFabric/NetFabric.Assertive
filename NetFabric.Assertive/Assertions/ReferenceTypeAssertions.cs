using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class ReferenceTypeAssertions<TActual> 
        : AssertionsBase<TActual>
        where TActual : class
    {
        internal ReferenceTypeAssertions(TActual actual) 
            : base(actual)
        {
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
                throw new NullException<TActual>();

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

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEnumerable<TActualItem>()
        {
            TypeExtensions.AssertIsEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new EnumerableReferenceTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
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