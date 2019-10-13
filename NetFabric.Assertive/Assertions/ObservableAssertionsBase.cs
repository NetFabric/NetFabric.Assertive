using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ObservableAssertionsBase<TActual, TActualItem> 
        : AssertionsBase<TActual> 
    {
        internal ObservableAssertionsBase(TActual actual) 
            : base(actual)
        {
        }
    }
}