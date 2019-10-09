using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AssertionsBase<TActual> 
    {
        internal AssertionsBase(TActual actual) 
        {
            Actual = actual;
        }

        public TActual Actual { get; }
    }
}