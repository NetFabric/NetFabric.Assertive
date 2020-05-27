using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AsyncEnumerableValueTypeAssertions<TActual, TActualItem> 
        : ValueTypeAssertionsBase<TActual>
        where TActual : struct
    {
        internal AsyncEnumerableValueTypeAssertions(TActual Actual, AsyncEnumerableInfo enumerableInfo)
            : base(Actual) 
            => EnumerableInfo = enumerableInfo;

        public AsyncEnumerableInfo EnumerableInfo { get; }

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> EvaluateTrue(Func<TActual, bool> func)
            => EvaluateTrue<AsyncEnumerableValueTypeAssertions<TActual, TActualItem>>(this, func);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> EvaluateFalse(Func<TActual, bool> func)
            => EvaluateFalse<AsyncEnumerableValueTypeAssertions<TActual, TActualItem>>(this, func);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeEmpty(bool deepComparison = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), deepComparison);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, bool deepComparison = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), deepComparison);

        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, bool deepComparison = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

            AssertAsyncEnumerableEquality(Actual, EnumerableInfo, expected, comparer);

            if (deepComparison)
                AssertDeepAsyncEnumerableEquality(Actual, expected, comparer);

            return this;
        }
    }
}