using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ObservableValueTypeAssertions<TActual, TActualItem>
        : ObservableAssertionsBase<TActual, TActualItem>
    {
        internal ObservableValueTypeAssertions(TActual actual)
            : base(actual)
        {
        }
    }
}