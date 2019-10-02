using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class ReadOnlyListAssertionsExtensions
    {
        public static ReadOnlyListAssertions<TActual, TActualItem> Must<TActual, TActualItem>(this TActual actual) 
            where TActual : IReadOnlyList<TActualItem>
            => new ReadOnlyListAssertions<TActual, TActualItem>(actual); 
    }

    [DebuggerNonUserCode]
    public class ReadOnlyListAssertions<TActual, TActualItem>
        : ReadOnlyCollectionAssertions<TActual, TActualItem>
        where TActual : IReadOnlyList<TActualItem>
    {
        readonly TActual actual;

        internal ReadOnlyListAssertions(TActual actual) 
            : base(actual) 
        {
            this.actual = actual;
        }
    }
}