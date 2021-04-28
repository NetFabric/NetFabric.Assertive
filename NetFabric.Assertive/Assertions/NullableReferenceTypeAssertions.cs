using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class NullableReferenceTypeAssertions<TActual> 
        : NullableReferenceTypeAssertionsBase<NullableReferenceTypeAssertions<TActual>, TActual>
        where TActual : class?
    {
        internal NullableReferenceTypeAssertions(TActual actual)
            : base(actual)
        { }
    }
}