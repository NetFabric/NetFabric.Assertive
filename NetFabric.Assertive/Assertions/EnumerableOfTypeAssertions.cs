using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableOfTypeAssertions<T> 
        : EnumerableAssertions
    {
        readonly IEnumerable<T> actual;

        internal EnumerableOfTypeAssertions(IEnumerable<T> actual) 
            : base(actual) 
        {
            this.actual = actual;
        }
    }
}