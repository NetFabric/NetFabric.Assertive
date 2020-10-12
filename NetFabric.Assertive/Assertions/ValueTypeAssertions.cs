using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ValueTypeAssertions<TActual> 
        : ValueTypeAssertionsBase<ValueTypeAssertions<TActual>, TActual>
        where TActual : struct
    {
        internal ValueTypeAssertions(TActual actual)
            : base(actual)
        { }
    }
}