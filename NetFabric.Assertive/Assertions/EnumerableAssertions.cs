using System;
using System.Collections;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableAssertions 
        : ObjectEnumerableAssertions
    {
        readonly IEnumerable actual;

        internal EnumerableAssertions(IEnumerable actual) 
            : base(actual, actual.GetType().GetEnumerableInfo()) 
        {
            this.actual = actual;
        }
    }
}