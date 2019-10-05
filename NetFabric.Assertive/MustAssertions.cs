using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class MustAssertions
    {
        [Pure]
        public static ObjectAssertions<object> Must(this object actual) 
            => new ObjectAssertions<object>(actual); 

        [Pure]
        public static ActionAssertions<Action> Must(this Action actual) 
            => new ActionAssertions<Action>(actual); 
    }
}