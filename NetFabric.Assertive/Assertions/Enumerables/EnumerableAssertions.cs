using System;
using System.Collections;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class EnumerableAssertionsExtensions
    {
        public static EnumerableAssertions<TActual> Must<TActual>(this TActual actual) 
            where TActual : IEnumerable
            => new EnumerableAssertions<TActual>(actual); 
    }

    [DebuggerNonUserCode]
    public class EnumerableAssertions<TActual> 
        : EnumerableObjectAssertions<TActual, object>
        where TActual : IEnumerable
    {
        readonly TActual actual;

        internal EnumerableAssertions(TActual actual) 
            : base(actual, actual.GetType().GetEnumerableInfo()) 
        {
            this.actual = actual;
        }
    }


}