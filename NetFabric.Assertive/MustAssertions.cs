using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class MustAssertions
    {
        public static ObjectAssertions<object> Must(this object actual) 
            => new ObjectAssertions<object>(actual); 

        public static ActionAssertions<Action> Must(this Action actual) 
            => new ActionAssertions<Action>(actual); 

        public static EnumerableAssertions<IEnumerable> Must(this IEnumerable actual) 
            => new EnumerableAssertions<IEnumerable>(actual); 
    }
}