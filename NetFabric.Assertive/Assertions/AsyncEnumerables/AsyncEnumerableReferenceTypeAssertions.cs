using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>
        : ReferenceTypeAssertionsBase<TActual>
        where TActual : class
    {
        internal AsyncEnumerableReferenceTypeAssertions(TActual Actual, AsyncEnumerableInfo enumerableInfo)
            : base(Actual) 
            => EnumerableInfo = enumerableInfo;

        public AsyncEnumerableInfo EnumerableInfo { get; }

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeNull()
            => BeNull<AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>>(this);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeNotNull()
            => BeNotNull<AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>>(this);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeSameAs<TOther>(TOther other)
            => BeSameAs<AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>, TOther>(this, other);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeNotSameAs<TOther>(TOther other)
            => BeNotSameAs<AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>, TOther>(this, other);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluateTrue(Func<TActual, bool> func)
            => EvaluateTrue<AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>>(this, func);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluateFalse(Func<TActual, bool> func)
            => EvaluateFalse<AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>>(this, func);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeEmpty(bool warnRefStructs = true, bool warnRefReturns = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), warnRefStructs, warnRefReturns);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, bool warnRefStructs = true, bool warnRefReturns = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), warnRefStructs, warnRefReturns);

        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, bool warnRefStructs = true, bool warnRefReturns = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

                AssertAsyncEnumerableEquality(Actual, EnumerableInfo, expected, comparer, warnRefStructs, warnRefReturns);
            }

            return this;
        }
    }
}