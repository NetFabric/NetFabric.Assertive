using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReadOnlyListAssertions<T>
        : ReadOnlyCollectionAssertions<T>
    {
        readonly IReadOnlyList<T> actual;

        internal ReadOnlyListAssertions(IReadOnlyList<T> actual) 
            : base(actual) 
        {
            this.actual = actual;
        }
    }
}