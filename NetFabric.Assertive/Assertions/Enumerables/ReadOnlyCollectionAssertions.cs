using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class ReadOnlyCollectionAssertionsExtensions
    {
        public static ReadOnlyCollectionAssertions<TActual, TActualItem> Must<TActual, TActualItem>(this TActual actual) 
            where TActual : IReadOnlyCollection<TActualItem>
            => new ReadOnlyCollectionAssertions<TActual, TActualItem>(actual); 
    }

    [DebuggerNonUserCode]
    public class ReadOnlyCollectionAssertions<TActual, TActualItem>
        : EnumerableOfTypeAssertions<TActual, TActualItem>
        where TActual : IReadOnlyCollection<TActualItem>
    {
        readonly TActual actual;

        internal ReadOnlyCollectionAssertions(TActual actual) 
            : base(actual) 
        {
            this.actual = actual;
        }

        protected override void EqualityComparison<TExpectedItem>(EnumerableWrapper<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
            base.EqualityComparison(actual, expected, (actual, expected) => equalityComparison((TActualItem)actual, (TExpectedItem)expected));

            // TODO: compare Count
        }
    }
}