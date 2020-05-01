using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public partial class ValueTypeAssertions<TActual> 
        : ValueTypeAssertionsBase<TActual>
        where TActual : struct
    {
        internal ValueTypeAssertions(TActual actual)
            : base(actual)
        {
        }

        public ValueTypeAssertions<TActual> BeDefault()
            => BeEqualTo<ValueTypeAssertions<TActual>>(this, default(TActual));

        public ValueTypeAssertions<TActual> EvaluateTrue(Func<TActual, bool> func)
            => EvaluateTrue<ValueTypeAssertions<TActual>>(this, func);

        public ValueTypeAssertions<TActual> EvaluateFalse(Func<TActual, bool> func)
            => EvaluateFalse<ValueTypeAssertions<TActual>>(this, func);

        public ValueTypeAssertions<TActual> BeOfType<TType>()
            => BeOfType<ValueTypeAssertions<TActual>, TType>(this);

        public ValueTypeAssertions<TActual> NotBeOfType<TType>()
            => NotBeOfType<ValueTypeAssertions<TActual>, TType>(this);

        public ValueTypeAssertions<TActual> BeAssignableTo<TType>()
            => BeAssignableTo<ValueTypeAssertions<TActual>, TType>(this);

        public ValueTypeAssertions<TActual> BeNotAssignableTo<TType>()
            => BeNotAssignableTo<ValueTypeAssertions<TActual>, TType>(this);

        public ValueTypeAssertions<TActual> BeEqualTo(TActual expected)
            => BeEqualTo<ValueTypeAssertions<TActual>>(this, expected);

        public ValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => BeEqualTo<ValueTypeAssertions<TActual>, TExpected>(this, expected, comparer);

        public ValueTypeAssertions<TActual> BeNotEqualTo(TActual expected)
            => BeNotEqualTo<ValueTypeAssertions<TActual>>(this, expected);

        public ValueTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => BeNotEqualTo<ValueTypeAssertions<TActual>, TExpected>(this, expected, comparer);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEnumerableOf<TActualItem>()
        {
            AssertIsEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new EnumerableValueTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeAsyncEnumerableOf<TActualItem>()
        {
            AssertIsAsyncEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new AsyncEnumerableValueTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }
    }
}