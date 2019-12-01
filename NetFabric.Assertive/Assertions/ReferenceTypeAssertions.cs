using System;
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

        public ReferenceTypeAssertions<TActual> EvaluateTrue(Func<TActual, bool> func)
            => this.EvaluateTrue<ReferenceTypeAssertions<TActual>, TActual>(func);

        public ReferenceTypeAssertions<TActual> EvaluateFalse(Func<TActual, bool> func)
            => this.EvaluateFalse<ReferenceTypeAssertions<TActual>, TActual>(func);

        [Obsolete("Use EvaluateTrue instead.")]
        public ReferenceTypeAssertions<TActual> EvaluatesTrue(Func<TActual, bool> func)
            => this.EvaluateTrue(func);

        [Obsolete("Use EvaluateFalse instead.")]
        public ReferenceTypeAssertions<TActual> EvaluatesFalse(Func<TActual, bool> func)
            => this.EvaluateFalse(func);

        public ReferenceTypeAssertions<TActual> BeOfType<TType>()
            => this.BeOfType<ReferenceTypeAssertions<TActual>, TActual, TType>();

        public ReferenceTypeAssertions<TActual> NotBeOfType<TType>()
            => this.NotBeOfType<ReferenceTypeAssertions<TActual>, TActual, TType>();

        public ReferenceTypeAssertions<TActual> BeAssignableTo<TType>()
            => this.BeAssignableTo<ReferenceTypeAssertions<TActual>, TActual, TType>();

        public ReferenceTypeAssertions<TActual> BeNotAssignableTo<TType>()
            => this.BeNotAssignableTo<ReferenceTypeAssertions<TActual>, TActual, TType>();

        public ReferenceTypeAssertions<TActual> BeNull()
            => this.BeNull<ReferenceTypeAssertions<TActual>, TActual>();

        public ReferenceTypeAssertions<TActual> BeNotNull()
            => this.BeNotNull<ReferenceTypeAssertions<TActual>, TActual>();

        public ReferenceTypeAssertions<TActual> BeSameAs<TExpected>(TExpected expected)
            => this.BeSameAs<ReferenceTypeAssertions<TActual>, TActual, TExpected>(expected);

        public ReferenceTypeAssertions<TActual> BeNotSameAs<TExpected>(TExpected expected)
            => this.BeNotSameAs<ReferenceTypeAssertions<TActual>, TActual, TExpected>(expected);

        public ReferenceTypeAssertions<TActual> BeEqualTo(TActual expected)
            => this.BeEqualTo<ReferenceTypeAssertions<TActual>, TActual>(expected);

        public ReferenceTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => this.BeEqualTo<ReferenceTypeAssertions<TActual>, TActual, TExpected>(expected, comparer);

        public ReferenceTypeAssertions<TActual> BeNotEqualTo(TActual expected)
            => this.BeNotEqualTo<ReferenceTypeAssertions<TActual>, TActual>(expected);

        public ReferenceTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => this.BeNotEqualTo<ReferenceTypeAssertions<TActual>, TActual, TExpected>(expected, comparer);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEnumerableOf<TActualItem>()
        {
            AssertIsEnumerable<TActualItem>(out var enumerableInfo);

            return new EnumerableReferenceTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeAsyncEnumerableOf<TActualItem>()
        {
            AssertIsAsyncEnumerable<TActualItem>(out var enumerableInfo);

            return new AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public ObservableReferenceTypeAssertions<TActual, TActualItem> BeObservableOf<TActualItem>()
        {
            var actualType = typeof(TActual);
            if (!actualType.IsAssignableTo(typeof(IObservable<>).MakeGenericType(typeof(TActualItem))))
                throw new ActualAssertionException<TActual>(Actual, $"Expected '{actualType}' to be an observable but doesn't implement 'IObservable <{typeof(TActualItem)}>'.");

            return new ObservableReferenceTypeAssertions<TActual, TActualItem>(Actual);
        }
    }
}