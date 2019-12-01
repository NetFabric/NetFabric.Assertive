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

        public ValueTypeAssertions<TActual> EvaluateTrue(Func<TActual, bool> func)
            => this.EvaluateTrue<ValueTypeAssertions<TActual>, TActual>(func);

        public ValueTypeAssertions<TActual> EvaluateFalse(Func<TActual, bool> func)
            => this.EvaluateFalse<ValueTypeAssertions<TActual>, TActual>(func);

        [Obsolete("Use EvaluateTrue instead.")]
        public ValueTypeAssertions<TActual> EvaluatesTrue(Func<TActual, bool> func)
            => this.EvaluateTrue(func);

        [Obsolete("Use EvaluateFalse instead.")]
        public ValueTypeAssertions<TActual> EvaluatesFalse(Func<TActual, bool> func)
            => this.EvaluateFalse(func);

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

        public ValueTypeAssertions<TActual> BeEqualTo(TActual expected)
            => this.BeEqualTo<ValueTypeAssertions<TActual>, TActual>(expected);

        public ValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => this.BeEqualTo<ValueTypeAssertions<TActual>, TActual, TExpected>(expected, comparer);

        public ValueTypeAssertions<TActual> BeNotEqualTo(TActual expected)
            => this.BeNotEqualTo<ValueTypeAssertions<TActual>, TActual>(expected);

        public ValueTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => this.BeNotEqualTo<ValueTypeAssertions<TActual>, TActual, TExpected>(expected, comparer);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEnumerableOf<TActualItem>()
        {
            AssertIsEnumerable<TActualItem>(out var enumerableInfo);

            return new EnumerableValueTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeAsyncEnumerableOf<TActualItem>()
        {
            AssertIsAsyncEnumerable<TActualItem>(out var enumerableInfo);

            return new AsyncEnumerableValueTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public ObservableValueTypeAssertions<TActual, TActualItem> BeObservableOf<TActualItem>()
        {
            var actualType = typeof(TActual);
            if (!actualType.IsAssignableTo(typeof(IObservable<>).MakeGenericType(typeof(TActualItem))))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{actualType}' to be an observable but doesn't implement 'IObservable <{typeof(TActualItem)}>'.");

            return new ObservableValueTypeAssertions<TActual, TActualItem>(Actual);
        }
    }
}