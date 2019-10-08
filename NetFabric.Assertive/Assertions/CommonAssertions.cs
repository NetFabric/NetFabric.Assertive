using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class CommonAssertions<TActual> 
    {
        internal CommonAssertions(TActual actual) 
        {
            Actual = actual;
        }

        public TActual Actual { get; }
    }
}