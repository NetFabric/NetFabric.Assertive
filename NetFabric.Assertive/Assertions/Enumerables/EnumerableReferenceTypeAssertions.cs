using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableReferenceTypeAssertions<TActual, TActualItem> 
        : EnumerableAssertionsBase<TActual>
        where TActual : class
    {
        internal EnumerableReferenceTypeAssertions(TActual actual, EnumerableInfo enumerableInfo) 
            : base(actual, enumerableInfo)
        {
        }

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluateTrue(Func<TActual, bool> func)
            => this.EvaluateTrue<EnumerableReferenceTypeAssertions<TActual, TActualItem>, TActual>(func);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluateFalse(Func<TActual, bool> func)
            => this.EvaluateFalse<EnumerableReferenceTypeAssertions<TActual, TActualItem>, TActual>(func);

        [Obsolete("Use EvaluateTrue instead.")]
        public EnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluatesTrue(Func<TActual, bool> func)
            => this.EvaluateTrue(func);

        [Obsolete("Use EvaluateFalse instead.")]
        public EnumerableReferenceTypeAssertions<TActual, TActualItem> EvaluatesFalse(Func<TActual, bool> func)
            => this.EvaluateFalse(func);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEmpty(bool deepComparison = true)
            => BeEqualTo(Enumerable.Empty<TActualItem>(), deepComparison);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected>(TExpected expected, bool deepComparison = true)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected), deepComparison);

        public EnumerableReferenceTypeAssertions<TActual, TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, bool deepComparison = true)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

                AssertEquality(expected, comparer);

                if (deepComparison)
                    AssertDeepEquality(expected, comparer);
            }

            return this;
        }
    }
}