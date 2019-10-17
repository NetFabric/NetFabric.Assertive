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

        public new ValueTypeAssertions<TActual> EvaluatesTrue(Func<TActual, bool> func)
            => this.EvaluatesTrue<ValueTypeAssertions<TActual>, TActual>(func);

        public new ValueTypeAssertions<TActual> EvaluatesFalse(Func<TActual, bool> func)
            => this.EvaluatesFalse<ValueTypeAssertions<TActual>, TActual>(func);

        public ValueTypeAssertions<TActual> BeOfType<TType>()
            => this.BeOfType<ValueTypeAssertions<TActual>, TActual, TType>();

        public ValueTypeAssertions<TActual> NotBeOfType<TType>()
            => this.NotBeOfType<ValueTypeAssertions<TActual>, TActual, TType>();

        public ValueTypeAssertions<TActual> BeAssignableTo<TType>()
            => this.BeAssignableTo<ValueTypeAssertions<TActual>, TActual, TType>();

        public ValueTypeAssertions<TActual> BeNotAssignableTo<TType>()
            => this.BeNotAssignableTo<ValueTypeAssertions<TActual>, TActual, TType>();

        public ValueTypeAssertions<TActual> BeDefault()
            => BeEqualTo(default);

        public ValueTypeAssertions<TActual> BeNotDefault()
            => BeNotEqualTo(default);

        public new ValueTypeAssertions<TActual> BeEqualTo(TActual expected)
            => this.BeEqualTo<ValueTypeAssertions<TActual>, TActual>(expected);

        public new ValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => this.BeEqualTo<ValueTypeAssertions<TActual>, TActual, TExpected>(expected, comparer);

        public new ValueTypeAssertions<TActual> BeNotEqualTo(TActual expected)
            => this.BeNotEqualTo<ValueTypeAssertions<TActual>, TActual>(expected);

        public new ValueTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => this.BeNotEqualTo<ValueTypeAssertions<TActual>, TActual, TExpected>(expected, comparer);

        public EnumerableValueTypeAssertions<TActual, KeyValuePair<TActualKey, TActualItem>> BeDictionary<TActualKey, TActualItem>()
            => BeEnumerable<KeyValuePair<TActualKey, TActualItem>>();

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEnumerable<TActualItem>()
        {
            TypeExtensions.AssertIsEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new EnumerableValueTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeAsyncEnumerable<TActualItem>()
        {
            TypeExtensions.AssertIsAsyncEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new AsyncEnumerableValueTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
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