using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableValueTypeAssertions<TActual, TActualItem> 
        : ValueTypeAssertionsBase<TActual>
        where TActual : struct
    {
        internal EnumerableValueTypeAssertions(TActual Actual, EnumerableInfo enumerableInfo)
            : base(Actual) 
            => EnumerableInfo = enumerableInfo;

        public EnumerableInfo EnumerableInfo { get; }

        public EnumerableValueTypeAssertions<TActual, TActualItem> EvaluateTrue(Func<TActual, bool> func)
            => EvaluateTrue<EnumerableValueTypeAssertions<TActual, TActualItem>>(this, func);

        public EnumerableValueTypeAssertions<TActual, TActualItem> EvaluateFalse(Func<TActual, bool> func)
            => EvaluateFalse<EnumerableValueTypeAssertions<TActual, TActualItem>>(this, func);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEmpty(bool warnRefStructs = true, bool warnRefReturns = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), warnRefStructs, warnRefReturns);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, bool warnRefStructs = true, bool warnRefReturns = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), warnRefStructs, warnRefReturns);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, bool warnRefStructs = true, bool warnRefReturns = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

            AssertEnumerableEquality(Actual, EnumerableInfo, expected, comparer, warnRefStructs, warnRefReturns);

            return this;
        }
    }
}