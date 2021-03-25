using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReferenceTypeAssertions<TActual> 
        : ReferenceTypeAssertionsBase<ReferenceTypeAssertions<TActual>, TActual>
        where TActual : class
    {
        internal ReferenceTypeAssertions(TActual actual)
            : base(actual)
        { }
    }
}