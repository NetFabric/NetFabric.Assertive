using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ObservableReferenceTypeAssertions<TActual, TActualItem> 
        : ObservableAssertionsBase<TActual, TActualItem>
    {
        internal ObservableReferenceTypeAssertions(TActual actual) 
            : base(actual)
        {
        }
    }
}