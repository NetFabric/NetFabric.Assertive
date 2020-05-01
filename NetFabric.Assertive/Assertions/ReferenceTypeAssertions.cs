using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReferenceTypeAssertions<TActual> 
        : ReferenceTypeAssertionsBase<TActual>
        where TActual : class
    {
        internal ReferenceTypeAssertions([AllowNull]TActual actual) 
            : base(actual)
        {
        }

        public ReferenceTypeAssertions<TActual> BeNull()
            => BeNull<ReferenceTypeAssertions<TActual>>(this);

        public ReferenceTypeAssertions<TActual> BeNotNull()
            => BeNotNull<ReferenceTypeAssertions<TActual>>(this);

        public ReferenceTypeAssertions<TActual> BeSameAs<TOther>(TOther other)
            => BeSameAs<ReferenceTypeAssertions<TActual>, TOther>(this, other);

        public ReferenceTypeAssertions<TActual> BeNotSameAs<TOther>(TOther other)
            => BeNotSameAs<ReferenceTypeAssertions<TActual>, TOther>(this, other);

        public ReferenceTypeAssertions<TActual> EvaluateTrue(Func<TActual, bool> func)
            => EvaluateTrue<ReferenceTypeAssertions<TActual>>(this, func);

        public ReferenceTypeAssertions<TActual> EvaluateFalse(Func<TActual, bool> func)
            => EvaluateFalse<ReferenceTypeAssertions<TActual>>(this, func);

        public ReferenceTypeAssertions<TActual> BeOfType<TType>()
            => BeOfType<ReferenceTypeAssertions<TActual>, TType>(this);

        public ReferenceTypeAssertions<TActual> NotBeOfType<TType>()
            => NotBeOfType<ReferenceTypeAssertions<TActual>, TType>(this);

        public ReferenceTypeAssertions<TActual> BeAssignableTo<TType>()
            => BeAssignableTo<ReferenceTypeAssertions<TActual>, TType>(this);

        public ReferenceTypeAssertions<TActual> BeNotAssignableTo<TType>()
            => BeNotAssignableTo<ReferenceTypeAssertions<TActual>, TType>(this);

        public ReferenceTypeAssertions<TActual> BeEqualTo(TActual expected)
            => BeEqualTo<ReferenceTypeAssertions<TActual>>(this, expected);

        public ReferenceTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => BeEqualTo<ReferenceTypeAssertions<TActual>, TExpected>(this, expected, comparer);

        public ReferenceTypeAssertions<TActual> BeNotEqualTo(TActual expected)
            => BeNotEqualTo<ReferenceTypeAssertions<TActual>>(this, expected);

        public ReferenceTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => BeNotEqualTo<ReferenceTypeAssertions<TActual>, TExpected>(this, expected, comparer);

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