using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class EnumerableOfTypeAssertionsExtensions
    {
        public static EnumerableOfTypeAssertions<TActual, TActualItem> Must<TActual, TActualItem>(this TActual actual) 
            where TActual : IEnumerable<TActualItem>
            => new EnumerableOfTypeAssertions<TActual, TActualItem>(actual); 
    }

    [DebuggerNonUserCode]
    public class EnumerableOfTypeAssertions<TActual, TActualItem> 
        : EnumerableAssertions<TActual>
        where TActual : IEnumerable<TActualItem>
    {
        readonly IEnumerable<TActualItem> actual;

        internal EnumerableOfTypeAssertions(TActual actual) 
            : base(actual) 
        {
            this.actual = actual;
        }
    }
}