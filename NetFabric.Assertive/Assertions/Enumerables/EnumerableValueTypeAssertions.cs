using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableValueTypeAssertions<TActual, TActualItem> 
        : EnumerableAssertionsBase<TActual>
        where TActual : struct
    {
        internal EnumerableValueTypeAssertions(TActual actual, EnumerableInfo enumerableInfo) 
            : base(actual, enumerableInfo)
        {
        }

        public EnumerableValueTypeAssertions<TActual, TActualItem> EvaluateTrue(Func<TActual, bool> func)
            => this.EvaluateTrue<EnumerableValueTypeAssertions<TActual, TActualItem>, TActual>(func);

        public EnumerableValueTypeAssertions<TActual, TActualItem> EvaluateFalse(Func<TActual, bool> func)
            => this.EvaluateFalse<EnumerableValueTypeAssertions<TActual, TActualItem>, TActual>(func);

        [Obsolete("Use EvaluateTrue instead.")]
        public EnumerableValueTypeAssertions<TActual, TActualItem> EvaluatesTrue(Func<TActual, bool> func)
            => this.EvaluateTrue(func);

        [Obsolete("Use EvaluateFalse instead.")]
        public EnumerableValueTypeAssertions<TActual, TActualItem> EvaluatesFalse(Func<TActual, bool> func)
            => this.EvaluateFalse(func);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEmpty(bool deepComparison = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), deepComparison);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, bool deepComparison = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), deepComparison);

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, bool deepComparison = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (expected is null)
                throw new ArgumentNullException(nameof(expected), $"{typeof(TActual)} is a value type so it can't be expected to be <null>.");

            AssertEquality(expected, comparer);

            if (deepComparison)
                AssertDeepEquality(expected, comparer);

            return this;
        }
    }
}